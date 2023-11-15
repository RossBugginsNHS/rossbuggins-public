namespace CommsCheck;
public interface ICommCheck
{
     Task Check(CommsCheckItemWithId toCheck,  CancellationToken cancellationToken = default);
}
