namespace CommsCheck;
using MediatR;

public class ItemNotFoundInCacheEventHandler(ICommCheck _check) : INotificationHandler<ItemNotFoundInCacheEvent>
{
    public async Task Handle(ItemNotFoundInCacheEvent notification, CancellationToken cancellationToken)
    {
        await _check.Check(notification.CommCheckCorrelationId, notification.Item, cancellationToken);
    }
}
