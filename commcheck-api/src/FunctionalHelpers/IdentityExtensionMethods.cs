
namespace FunctionalHelpers
{
    public static class IdentityExtensionMethods
    {
        public static Identity<T> ToIdentity<T>(this T @this) => new Identity<T>(@this);
    }
}