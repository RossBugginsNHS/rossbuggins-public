namespace CommsCheck;
public class CommsCheckNativeRulesOptions(IServiceCollection services)
{

    public CommsCheckNativeRulesOptions AddRule<T>() where T : class, ICommCheckRule
    {
        services.AddTransient<ICommCheckRule, T>();
        return this;
    }

    public CommsCheckNativeRulesOptions AddRule<U, T>()
        where T : class, ICommCheckRule<U>
        where U : IContactType
    {
        services.AddTransient<ICommCheckRule<U>, T>();
        return this;
    }
}
