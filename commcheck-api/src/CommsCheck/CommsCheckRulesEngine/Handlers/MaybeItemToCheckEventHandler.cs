namespace CommsCheck;

using System.Diagnostics;
using System.Diagnostics.Metrics;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

public class MaybeItemToCheckEventHandler(
    IPublisher _publisher,
    ILogger<MaybeItemToCheckEventHandler> _logger,
    IDistributedCache _cache
) : INotificationHandler<MaybeItemToCheckEvent>
{
    private static readonly Meter MyMeter = new("NHS.CommChecker.CommsCheckHostedService", "1.0");
    private static readonly Counter<long> ProcessCheckCount = MyMeter.CreateCounter<long>("ProcessCheck_Count");
    private static readonly Histogram<double> ProcessTime = MyMeter.CreateHistogram<double>("ProcessCheck_Duration_Seconds");
    private static readonly UpDownCounter<long> CurrentlyProcessing = MyMeter.CreateUpDownCounter<long>("ProcessCheck_Active_Count");

    public async Task Handle(MaybeItemToCheckEvent notification, CancellationToken cancellationToken)
    {
        await TryProcessCommCheckItem(notification.CommCheckCorrelationId, notification.Item);
    }

    private async Task TryProcessCommCheckItem(Guid commCheckCorrelationId, CommsCheckItemWithId item)
    {
        var sw = MetricStart();
        try
        {
            await ProcessCommCheckItem(commCheckCorrelationId, item);
        }
        catch (Exception e)
        {
            ProcessException(commCheckCorrelationId, e, item);
        }
        finally
        {
            MetricsStop(sw);
        }
    }

    private void ProcessException(Guid commCheckCorrelationId, Exception ex, CommsCheckItemWithId item)
    {
        _logger.LogError(
            ex, 
            "[{commCheckCorrelationId}] An unhandled exception occu whilst proccessing the comms check item {item}", 
            commCheckCorrelationId,
            item);
    }

    private static Stopwatch MetricStart()
    {
        var sw = Stopwatch.StartNew();
        CurrentlyProcessing.Add(1);
        return sw;
    }

    private static void MetricsStop(Stopwatch sw)
    {
        sw.Stop();
        ProcessTime.Record(sw.Elapsed.TotalSeconds);
        CurrentlyProcessing.Add(-1);
    }

    private async Task ProcessCommCheckItem(Guid commCheckCorrelationId, CommsCheckItemWithId item)
    {
        if (await IsNotInCache(item.Id))
        {
            await ProcessCheck(commCheckCorrelationId, item);
        }
    }

    private async Task<bool> IsNotInCache(string id)
    {
        var cacheEntry = await _cache.GetAsync(id);
        return cacheEntry == null;
    }

    private async Task ProcessCheck(Guid commCheckCorrelationId, CommsCheckItemWithId item)
    {
        await _publisher.Publish(new ItemToCheckEvent(commCheckCorrelationId, item));
        ProcessCheckCount.Add(1);
    }
}
