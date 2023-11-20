namespace CommsCheck;
public class ItemNotFoundInCacheEvent (Guid commCheckCorrelationId, CommsCheckItemWithId item): 
    ICommsCheckEvent
{
    public CommsCheckItemWithId Item => item;
    public Guid CommCheckCorrelationId => commCheckCorrelationId;
}
