namespace CommsCheck;
using MediatR;

public class ItemToCheckEventHandler(ICommCheck _check) : INotificationHandler<ItemToCheckEvent>
{
    

    public async Task Handle(ItemToCheckEvent notification, CancellationToken cancellationToken)
    {
        await _check.Check(notification.Item, cancellationToken);
    }
}
