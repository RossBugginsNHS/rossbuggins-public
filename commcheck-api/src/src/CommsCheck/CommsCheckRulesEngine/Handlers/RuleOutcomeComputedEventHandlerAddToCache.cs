namespace CommsCheck;

using MediatR;

public class RuleOutcomeComputedEventHandlerAddToCache(RuleRunMethodResultCacheService _cache) 
: INotificationHandler<RuleOutcomeComputedEvent>
{
    public async Task Handle(RuleOutcomeComputedEvent notification, CancellationToken cancellationToken)
    {
        await _cache.NewOrUpdateCacheThreadSafe(notification, cancellationToken);
    }
}
