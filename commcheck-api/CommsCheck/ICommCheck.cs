public interface ICommCheck
{
     Task<CommsCheckAnswer> Check(CommsCheckItem toCheck);
}
