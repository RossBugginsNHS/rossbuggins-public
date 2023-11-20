namespace CommsCheck;

using System.Collections.Concurrent;
using MediatR;

public class RuleOutcomeComputedEventHandlerMostRecent100(MostRecent100Cache _cache)
: INotificationHandler<ItemCacheUpdatedEvent>
{
    public async Task Handle(ItemCacheUpdatedEvent notification, CancellationToken cancellationToken)
    {
        await _cache.Add(notification);
    }
}
