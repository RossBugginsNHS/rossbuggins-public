using MediatR;

public class HostedServiceMaybeToCheckItemEvent(CommsCheckItemWithId item) : INotification
{
    public CommsCheckItemWithId Item => item;
}
