using Microsoft.Extensions.Caching.Distributed;

public class RuleRunMethodResultCacheService(IDistributedCache _cache)
{
    private SemaphoreSlim _slim = new SemaphoreSlim(1,1);
    public async Task HandleLock(RuleRunMethodResultEvent notification, CancellationToken cancellationToken)
    {
        await _slim.WaitAsync(cancellationToken);
        try
        {
        var cacheItem = await _cache.GetAsync(notification.ToCheck.Id);
        if (cacheItem == null)
        {
            var newAnswer = new CommsCheckAnswer(
                notification.ToCheck.Id,
                notification.ToCheck.ToString(),
                notification.Outcome);

            var b = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(newAnswer);
            await _cache.SetAsync(notification.ToCheck.Id, b);
        }
        else
        {
            var exitingItem = System.Text.Json.JsonSerializer.Deserialize<CommsCheckAnswer>(cacheItem);
            var updatedItem = exitingItem with {Outcomes = exitingItem.Outcomes.Append(notification.Outcome).ToArray()};
            var b = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(updatedItem);
            await _cache.SetAsync(notification.ToCheck.Id, b);
        }
        }
        finally
        {
            _slim.Release();
        }
    }
}
