
namespace FunctionalHelpers
{
    public class Full<T> : Maybe<T>
    {
        public Full(T value)
        {
            Value = value;
        }
        public virtual T Value { get;}
    }
}