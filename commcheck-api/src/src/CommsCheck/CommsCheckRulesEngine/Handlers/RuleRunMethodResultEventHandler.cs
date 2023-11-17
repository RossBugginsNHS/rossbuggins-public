namespace CommsCheck;

using MediatR;

public class RuleRunMethodResultEventHandler(RuleRunMethodResultCacheService _cache) 
: INotificationHandler<RuleRunMethodResultEvent>
{
    public async Task Handle(RuleRunMethodResultEvent notification, CancellationToken cancellationToken)
    {
        await _cache.NewOrUpdateCacheThreadSafe(notification, cancellationToken);
    }
}
