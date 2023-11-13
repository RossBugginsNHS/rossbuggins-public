using MediatR;

public class HostedServiceCheckItemEvent (CommsCheckItemWithId item): INotification
{
    public CommsCheckItemWithId Item => item;
}
