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
        await TryProcessCommCheckItem(notification.Item);
    }

    private async Task TryProcessCommCheckItem(CommsCheckItemWithId item)
    {
        var sw = MetricStart();
        try
        {
            await ProcessCommCheckItem(item);
        }
        catch (Exception e)
        {
            ProcessException(e);
        }
        finally
        {
            MetricsStop(sw);
        }
    }

    private void ProcessException(Exception ex)
    {
        _logger.LogError(ex, "An unhandled exception occured");
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

    private async Task ProcessCommCheckItem(CommsCheckItemWithId item)
    {
        if (await IsNotInCache(item.Id))
        {
            await ProcessCheck(item);
        }
    }

    private async Task<bool> IsNotInCache(string id)
    {
        var cacheEntry = await _cache.GetAsync(id);
        return cacheEntry == null;
    }

    private async Task ProcessCheck(CommsCheckItemWithId item)
    {
        await _publisher.Publish(new ItemToCheckEvent(item));
        ProcessCheckCount.Add(1);
    }
}
