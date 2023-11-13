using MediatR;

public class RuleRunMethodResultEventHandler(RuleRunMethodResultCacheService _cache) 
: INotificationHandler<RuleRunMethodResultEvent>
{
    public async Task Handle(RuleRunMethodResultEvent notification, CancellationToken cancellationToken)
    {
        await _cache.HandleLock(notification, cancellationToken);
    }
}
