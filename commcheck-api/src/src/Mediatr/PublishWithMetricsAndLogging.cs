namespace CommsCheck;

using System.Diagnostics;
using System.Diagnostics.Metrics;
using MediatR;

public class PublishWithMetricsAndLogging : Mediator
{
    private static readonly Meter MyMeter = new("NHS.CommChecker.MyPublish", "1.0");
    private static readonly Counter<long> HandledCounter = MyMeter.CreateCounter<long>("MyPublish_Handled_Count");
    private static readonly Histogram<double> ProcessTime = MyMeter.CreateHistogram<double>("MyPublish_Duration_Seconds");
    private static readonly UpDownCounter<long> CurrentlyProcessing = MyMeter.CreateUpDownCounter<long>("MyPublish_Active_Count");

    private readonly ILogger<PublishWithMetricsAndLogging> _logger;
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
        if (notification is ICommsCheckEvent cce)
        {
            await Publish(handlerExecutors, cce, cancellationToken);
        }
        else
        {
            await base.PublishCore(handlerExecutors, notification, cancellationToken);
        }

    }

    public async Task Publish(
      IEnumerable<NotificationHandlerExecutor> handlerExecutors,
      ICommsCheckEvent notification,
      CancellationToken cancellationToken)
    {
        var executorsList = handlerExecutors.ToList();
        
        PreRunMetricsLogging(executorsList, notification);
        foreach (var handler in executorsList)
        {
            var sw = StartMetrics(handler, notification);
            try
            {
                await handler.HandlerCallback(notification, cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                EndMetrics(sw, handler, notification);
            }
        }
    }

    private void PreRunMetricsLogging(
        IList<NotificationHandlerExecutor> handlerExecutors,
        ICommsCheckEvent notification)
    {
        _logger.LogDebug(
            "[{CommCheckCorrelationId}] [NOTIFICATION HANDLER COUNT: {notificationName}] => [{executorsCount}] {notification}",
            notification.CommCheckCorrelationId,
            notification.GetType().Name,
            handlerExecutors.Count,
            notification);
    }

    private void EndMetrics(Stopwatch sw,
        NotificationHandlerExecutor handler,
        ICommsCheckEvent notification)
    {
        sw.Stop();
        
        _logger.LogDebug(
            "[{CommCheckCorrelationId}] [NOTIFICATION HANDLER FINISHED: {notificationName}] => [{handlerName}] {notification}",
            notification.CommCheckCorrelationId,
            notification.GetType().Name,
            handler.HandlerInstance.GetType().Name,
            notification);

        ProcessTime.Record(sw.Elapsed.TotalSeconds,
            new KeyValuePair<string, object?>("notificationHandler", handler.HandlerInstance.GetType().Name),
            new KeyValuePair<string, object?>("notification", notification.GetType().Name));

        CurrentlyProcessing.Add(
            -1,

            new KeyValuePair<string, object?>("notificationHandler", handler.HandlerInstance.GetType().Name),
            new KeyValuePair<string, object?>("notification", notification.GetType().Name));
    }

    private Stopwatch StartMetrics(
        NotificationHandlerExecutor handler,
        ICommsCheckEvent notification)
    {
        var sw = Stopwatch.StartNew();

        HandledCounter.Add(
            1,
            new KeyValuePair<string, object?>("notificationHandler", handler.HandlerInstance.GetType().Name),
            new KeyValuePair<string, object?>("notification", notification.GetType().Name));

        CurrentlyProcessing.Add(
            1,
            new KeyValuePair<string, object?>("notificationHandler", handler.HandlerInstance.GetType().Name),
            new KeyValuePair<string, object?>("notification", notification.GetType().Name));

        _logger.LogDebug(
            "[{CommCheckCorrelationId}] [NOTIFICATION HANDLER STARTING: {notificationName}] => [{handlerName}] {notification}",
            notification.CommCheckCorrelationId,
            notification.GetType().Name,
            handler.HandlerInstance.GetType().Name,
            notification);

        return sw;
    }
}