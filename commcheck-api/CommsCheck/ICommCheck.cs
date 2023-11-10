public interface ICommCheck
{
     Task<CommsCheckAnswer> Check(CommsCheckItemWithId toCheck);
}
