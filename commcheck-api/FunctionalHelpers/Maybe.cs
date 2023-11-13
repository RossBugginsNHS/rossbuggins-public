namespace FunctionalHelpers
{
    public abstract class Maybe<T>
    {
        public virtual T Value { get; protected set; }
    }
}