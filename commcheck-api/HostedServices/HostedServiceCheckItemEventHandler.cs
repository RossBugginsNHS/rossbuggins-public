using MediatR;

public class HostedServiceCheckItemEventHandler(ICommCheck _check) : INotificationHandler<HostedServiceCheckItemEvent>
{
    public async Task Handle(HostedServiceCheckItemEvent notification, CancellationToken cancellationToken)
    {
        await _check.Check(notification.Item);
    }
}