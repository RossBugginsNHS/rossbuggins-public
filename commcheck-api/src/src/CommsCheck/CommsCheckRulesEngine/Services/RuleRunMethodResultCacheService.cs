
namespace CommsCheck;
using System.Collections;
using System.Data.SqlTypes;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Caching.Distributed;
using FunctionalHelpers;
using System.Text.Json;
using MediatR;
public class RuleRunMethodResultCacheService
{
    private readonly ILogger<RuleRunMethodResultCacheService> _logger;
    private readonly IDistributedCache _cache;
    private readonly SemaphoreSlim _slim;
    private readonly Func<string, Task<byte[]?>> _getFromCache;

    private readonly IPublisher _publisher;

    private readonly TimeProvider _timeProvider;

    public RuleRunMethodResultCacheService(
        TimeProvider timeProvider,
        IPublisher publisher,
        IDistributedCache cache,
        ILogger<RuleRunMethodResultCacheService> logger)
    {
        _timeProvider= timeProvider;
        _publisher= publisher;
        _logger = logger;
        _slim = new SemaphoreSlim(1, 1);
        _cache = cache;
        _getFromCache = async x => await _cache.GetAsync(x);
    }

    public async Task NewOrUpdateCacheThreadSafe(
        RuleOutcomeComputedEvent notification, 
        CancellationToken cancellationToken)
    {
        CommsCheckAnswer? answer;
        //With no eventsouring (or other shared datastore) need to lock this as no concurrency checks.
        // Obv will have problems on multiple instances, so would need to do the writing to event store here
        // and then a separate service which streams the events and updates cache accordingly.
        await _slim.WaitAsync(cancellationToken);
        try
        {
            var maybe = await GetMaybeIfCacheItemExists(notification.ToCheck.Id);
            answer = await NewOrUpdateCacheItem(maybe, notification);
        }
        finally
        {
            _slim.Release();
        }

        ArgumentNullException.ThrowIfNull(answer);

        await _publisher.Publish(new ItemCacheUpdatedEvent(answer.Value), cancellationToken);

        _logger.LogInformation(
            "[{correlationId}] Answer updated {answer}",
            notification.CommCheckCorrelationId,
            CommsCheckAnswerResponseDto.FromCommsCheckAnswer(answer.Value)
            );

    }

    private Task<CommsCheckAnswer> NewOrUpdateCacheItem(
        Maybe<byte[]?> maybe, 
        RuleOutcomeComputedEvent notification)
    {
        //curry the functions
        Func<Task<CommsCheckAnswer>> newItem = () => WriteNewItem(notification);
        Func<byte[]?, Task<CommsCheckAnswer>> updateItem = x => WriteUpdatedItem(x, notification);

        var rVal = maybe.Fork(
            newItem,
            full => updateItem(full));

        return rVal;
    }

    private async Task<Maybe<byte[]?>> GetMaybeIfCacheItemExists(string id) =>
            await
            id.ToIdentity()
            .MaybeAsync(async (_id) => await _getFromCache(_id));


    private async Task<CommsCheckAnswer> WriteNewItem(RuleOutcomeComputedEvent notification)
    {
        var newAnswer = BuildNewItem(notification);

        var b = JsonSerializer.SerializeToUtf8Bytes(newAnswer);
        await _cache.SetAsync(notification.ToCheck.Id, b);
        return newAnswer;
    }

    private CommsCheckAnswer BuildNewItem(RuleOutcomeComputedEvent notification) =>
    new CommsCheckAnswer(
            notification.ToCheck.Id,
            notification.ToCheck.Item.CopyOfSource,
            notification.ToCheck.Item.UtcDateCheckItemCreated,
            GetNow(),
            GetNow(),
            1,
            notification.Outcome);

    private CommsCheckAnswer BuildUpdatedItem(
        RuleOutcomeComputedEvent notification,
        CommsCheckAnswer existingItem) =>
        existingItem with
        {
            Outcomes = existingItem.Outcomes.Append(notification.Outcome).ToArray(),
            UpdatedCount = existingItem.UpdatedCount + 1,
            UpdatedAt = GetNow()
        };


    private async Task<CommsCheckAnswer> WriteUpdatedItem(
        byte[]? bytesIn, 
        RuleOutcomeComputedEvent notification)
    {
        byte[] existingBytes = Array.Empty<byte>();
        if (bytesIn != null)
            existingBytes = bytesIn;

        var exitingItem = JsonSerializer.Deserialize<CommsCheckAnswer>(existingBytes);
        var updatedItem = BuildUpdatedItem(notification, exitingItem);

        var b = JsonSerializer.SerializeToUtf8Bytes(updatedItem);
        await _cache.SetAsync(notification.ToCheck.Id, b);
        return updatedItem;
    }

    private DateTime GetNow() => _timeProvider.GetUtcNow().UtcDateTime;
}