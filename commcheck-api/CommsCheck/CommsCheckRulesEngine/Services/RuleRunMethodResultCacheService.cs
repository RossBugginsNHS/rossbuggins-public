using System.Collections;
using System.Data.SqlTypes;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Caching.Distributed;
using FunctionalHelpers;

public class RuleRunMethodResultCacheService(IDistributedCache _cache)
{
    private SemaphoreSlim _slim = new SemaphoreSlim(1, 1);

    private Func<string, Task<byte[]?>> _getFromCache = async x => await _cache.GetAsync(x);
    public async Task NewOrUpdateCacheThreadSafe(RuleRunMethodResultEvent notification, CancellationToken cancellationToken)
    {
        //With no eventsouring (or other shared datastore) need to lock this as no concurrency checks.
        // Obv will have problems on multiple instances, so would need to do the writing to event store here
        // and then a separate service which streams the events and updates cache accordingly.
        await _slim.WaitAsync(cancellationToken);
        try
        {
            var maybe = await GetMaybeIfCacheItemExists(notification.ToCheck.Id);
            await NewOrUpdateCacheItem(maybe, notification);
        }
        finally
        {
            _slim.Release();
        }
    }

    private Task NewOrUpdateCacheItem(Maybe<byte[]?> maybe, RuleRunMethodResultEvent notification)
    {
        //curry the functions
        Func<byte[]?, Task> newItem = x => WriteNewItem(notification);
        Func<byte[]?, Task> updateItem = x => WriteUpdatedItem(x, notification);

        var rVal = maybe.Fork(
            empty => newItem(empty),
            full => updateItem(full));

        return rVal;
    }

    private async Task<Maybe<byte[]?>> GetMaybeIfCacheItemExists(string id) =>
            await
            id.ToIdentity()
            .MaybeAsync(async (_id) => await _getFromCache(_id));


    private async Task WriteNewItem(RuleRunMethodResultEvent notification)
    {
        var newAnswer = new CommsCheckAnswer(
            notification.ToCheck.Id,
            notification.ToCheck.ToString(),
            notification.Outcome);

        var b = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(newAnswer);
        await _cache.SetAsync(notification.ToCheck.Id, b);
    }

    private async Task WriteUpdatedItem(byte[] existingBytes, RuleRunMethodResultEvent notification)
    {
        var exitingItem = System.Text.Json.JsonSerializer.Deserialize<CommsCheckAnswer>(existingBytes);
        var updatedItem = exitingItem with { Outcomes = exitingItem.Outcomes.Append(notification.Outcome).ToArray() };
        var b = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(updatedItem);
        await _cache.SetAsync(notification.ToCheck.Id, b);
    }
}