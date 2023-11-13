using MediatR;

public class ItemToCheckEvent (CommsCheckItemWithId item): ICommsCheckEvent
{
    public CommsCheckItemWithId Item => item;
}


public interface ICommsCheckEvent : INotification
{

}