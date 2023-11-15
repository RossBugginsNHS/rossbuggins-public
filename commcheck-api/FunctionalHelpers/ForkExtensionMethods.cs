
namespace FunctionalHelpers
{
    public static class ForkExtensionMethods
    {
        public static Maybe<TOutputType> Maybe<TInputType, TOutputType>(
            this Identity<TInputType> @this, Func<Identity<TInputType>, TOutputType> f)
        {
            var result = f(@this);

            Maybe<TOutputType> maybe = EqualityComparer<TOutputType>.Default.Equals(result, default) ?
                new Empty<TOutputType>() :
                new Full<TOutputType>(result);

            return maybe;
        }

        public static async Task<Maybe<TOutputType>> MaybeAsync<TInputType, TOutputType>(
        this Identity<TInputType> @this, Func<Identity<TInputType>, Task<TOutputType>> f)
        {
            var result = await f(@this);

            Maybe<TOutputType> maybe = EqualityComparer<TOutputType>.Default.Equals(result, default) ?
                new Empty<TOutputType>() :
                new Full<TOutputType>(result);

            return maybe;
        }

        public static TOutputType Fork<TInputType, TOutputType>(
        this Maybe<TInputType> @this, 
        Func<TOutputType> fEmpty, 
        Func<TInputType, TOutputType> fFull)
        {
            var rVal = @this switch
            {
                Empty<TInputType> empty => fEmpty(),
                Full<TInputType> full => fFull(full.Value),
                _ => throw new ArgumentOutOfRangeException()
            };

            return rVal;
        }
    }
}