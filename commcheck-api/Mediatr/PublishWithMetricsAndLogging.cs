using System.Diagnostics;
using System.Diagnostics.Metrics;
using MediatR;

public class PublishWithMetricsAndLogging : Mediator
{
    private static readonly Meter MyMeter = new("NHS.CommChecker.MyPublish", "1.0");
    private static readonly Counter<long> HandledCounter = MyMeter.CreateCounter<long>("MyPublish_Handled_Count");
    private static readonly Histogram<double> ProcessTime = MyMeter.CreateHistogram<double>("MyPublish_Duration_Seconds");
    private static readonly UpDownCounter<long> CurrentlyProcessing = MyMeter.CreateUpDownCounter<long>("MyPublish_Active_Count");

    ILogger<PublishWithMetricsAndLogging> _logger;
    public PublishWithMetricsAndLogging(
     ILogger<PublishWithMetricsAndLogging> logger,
     IServiceProvider serviceFactory)
     : base(serviceFactory)
    {
        _logger = logger;
    }

    protected override async Task PublishCore(
        IEnumerable<NotificationHandlerExecutor> handlerExecutors,
        INotification notification,
        CancellationToken cancellationToken)
    {
        await Publish(handlerExecutors, notification, cancellationToken);
    }

    public async Task Publish(
      IEnumerable<NotificationHandlerExecutor> handlerExecutors,
      INotification notification,
      CancellationToken cancellationToken)
    {
        var executorsList = handlerExecutors.ToList();
        _logger.LogInformation($"[NOTIFICATION HANDLER COUNT: {notification.GetType().Name}] => [{executorsList.Count}] {notification} ");
        foreach (var handler in executorsList)
        {
            var sw = Stopwatch.StartNew();

            HandledCounter.Add(
                1,
                new KeyValuePair<string, object>("notificationHandler", handler.HandlerInstance.GetType().Name),
                new KeyValuePair<string, object>("notification", notification.GetType().Name));

            CurrentlyProcessing.Add(
                1,
                new KeyValuePair<string, object>("notificationHandler", handler.HandlerInstance.GetType().Name),
                new KeyValuePair<string, object>("notification", notification.GetType().Name));

            _logger.LogInformation($"[NOTIFICATION HANDLER STARTING: {notification.GetType().Name}] => [{handler.HandlerInstance.GetType().Name}] {notification} ");

            try
            {
                await handler.HandlerCallback(notification, cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                sw.Stop();
                _logger.LogInformation($"[NOTIFICATION HANDLER FINISHED: {notification.GetType().Name}] => [{handler.HandlerInstance.GetType().Name}] {notification} ");

                ProcessTime.Record(sw.Elapsed.TotalSeconds,
                    new KeyValuePair<string, object>("notificationHandler", handler.HandlerInstance.GetType().Name),
                    new KeyValuePair<string, object>("notification", notification.GetType().Name));

                CurrentlyProcessing.Add(
                    -1,
                    new KeyValuePair<string, object>("notificationHandler", handler.HandlerInstance.GetType().Name),
                    new KeyValuePair<string, object>("notification", notification.GetType().Name));
            }
        }
    }
}