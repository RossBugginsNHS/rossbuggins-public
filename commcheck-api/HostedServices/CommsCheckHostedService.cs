using System.Threading.Channels;
using Microsoft.Extensions.Caching.Distributed;

public class CommsCheckHostedService(
    CommsCheckItemSha sha,
    ILogger<CommsCheckHostedService> _logger,
    ChannelReader<CommsCheckItem> _reader,
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

    private async Task TryProcessCommCheckItem(CommsCheckItem item)
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

    private async Task ProcessCommCheckItem(CommsCheckItem item)
    {
        var id = GetId(item);
        if (await IsNotInCache(id))
        {
            await ProcessCheck(id, item);
        }
    }

    private string GetId(CommsCheckItem item) =>
        sha.GetSha(item);

    private async Task<bool> IsNotInCache(string id)
    {
        var cacheEntry = await _cache.GetAsync(id);
        return cacheEntry == null;
    }
    private async Task ProcessCheck(string id, CommsCheckItem item)
    {
        var answer = await _check.Check(item);
        await AddToCache(id, answer);
    }

    private async Task AddToCache(string id, CommsCheckAnswer answer)
    {
        var b = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(answer);
        await _cache.SetAsync(id, b);
    }
}
