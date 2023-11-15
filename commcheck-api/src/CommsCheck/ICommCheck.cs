namespace CommsCheck;
public interface ICommCheck
{
     Task Check(Guid commCheckCorrelationId, CommsCheckItemWithId toCheck,  CancellationToken cancellationToken = default);
}
