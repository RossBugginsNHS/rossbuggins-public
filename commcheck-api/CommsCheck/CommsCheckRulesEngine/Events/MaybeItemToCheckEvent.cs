namespace CommsCheck;

using MediatR;

public class MaybeItemToCheckEvent(CommsCheckItemWithId item) : ICommsCheckEvent
{
    public CommsCheckItemWithId Item => item;
}
