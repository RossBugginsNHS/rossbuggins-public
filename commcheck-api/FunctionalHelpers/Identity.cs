namespace FunctionalHelpers
{
    public class Identity<T>
    {
        public T Value { get; }

        public Identity(T value) => Value = value;

        public static implicit operator T(Identity<T> @this) => @this.Value;
    }
}