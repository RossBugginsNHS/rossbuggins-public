namespace CommsCheck;
public interface ICommCheck
{
     Task Check(CommsCheckItemWithId toCheck);
}
