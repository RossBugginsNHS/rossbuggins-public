namespace CommsCheck;
public class ItemToCheckEvent (Guid commCheckCorrelationId, CommsCheckItemWithId item): 
    ICommsCheckEvent
{
    public CommsCheckItemWithId Item => item;
    public Guid CommCheckCorrelationId => commCheckCorrelationId;
}
