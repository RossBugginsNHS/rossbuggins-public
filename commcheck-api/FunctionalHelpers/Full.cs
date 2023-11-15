
namespace FunctionalHelpers
{
    public class Full<T> : Maybe<T>
    {
        public Full(T value)
        {
            Value = value;
        }
    }
}