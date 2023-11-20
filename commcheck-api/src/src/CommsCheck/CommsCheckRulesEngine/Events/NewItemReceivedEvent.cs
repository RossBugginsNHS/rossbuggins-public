namespace CommsCheck;

using MediatR;

public class NewItemReceivedEvent(Guid commCheckCorrelationId, CommsCheckItemWithId item) : ICommsCheckEvent
{
    public Guid CommCheckCorrelationId => commCheckCorrelationId;
    public CommsCheckItemWithId Item => item;
}
