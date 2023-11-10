using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Threading.Channels;
using Microsoft.Extensions.Caching.Distributed;

public class CommsCheckHostedService(
    ILogger<CommsCheckHostedService> _logger,
    ChannelReader<CommsCheckItemWithId> _reader,
    IDistributedCache _cache,
    ICommCheck _check) : BackgroundService
{

    private static readonly Meter MyMeter = new("NHS.CommChecker.CommsCheckHostedService", "1.0");
    private static readonly Counter<long> ProcessCheckCount = 
        MyMeter.CreateCounter<long>("ProcessCheck_Count");

    private static readonly System.Diagnostics.Metrics.Histogram<double> ProcessTime = 
        MyMeter.CreateHistogram<double>("ProcessCheck_Duration_Seconds");

    private static readonly System.Diagnostics.Metrics.UpDownCounter<long> CurrentlyProcessing = 
        MyMeter.CreateUpDownCounter<long>("ProcessCheck_Active_Count");

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var item in _reader.ReadAllAsync(stoppingToken))
        {
            await TryProcessCommCheckItem(item);
        }
    }

    private async Task TryProcessCommCheckItem(CommsCheckItemWithId item)
    {
         var sw = Stopwatch.StartNew();
        try
        {
            CurrentlyProcessing.Add(1);
            await ProcessCommCheckItem(item);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An unhandled exception occured");
        }
        finally
        {
            sw.Stop();
            ProcessTime.Record(sw.Elapsed.TotalSeconds);
            CurrentlyProcessing.Add(-1);
        }
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
        var answer = await _check.Check(item);
        ProcessCheckCount.Add(1);
        await AddToCache(item.Id, answer);
    }

    private async Task AddToCache(string id, CommsCheckAnswer answer)
    {
        var b = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(answer);
        await _cache.SetAsync(id, b);
    }
}
