using System.Threading.Channels;
using Microsoft.Extensions.Caching.Distributed;

public class CommsCheckHostedService(
    CommsCheckItemSha sha,
    ILogger<CommsCheckHostedService> _logger,
    ChannelReader<CommsCheckItemWithId> _reader,
    IDistributedCache _cache,
    ICommCheck _check) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var item in _reader.ReadAllAsync(stoppingToken))
        {
            await TryProcessCommCheckItem(item);
        }
    }

    private async Task TryProcessCommCheckItem(CommsCheckItemWithId item)
    {
        try
        {
            await ProcessCommCheckItem(item);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An unhandled exception occured");
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
        await AddToCache(item.Id, answer);
    }

    private async Task AddToCache(string id, CommsCheckAnswer answer)
    {
        var b = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(answer);
        await _cache.SetAsync(id, b);
    }
}
