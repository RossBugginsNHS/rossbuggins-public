namespace CommsCheck;

public static class Rule100ListExtensionMethods
{
    public static IServiceCollection AddRule100(this IServiceCollection services)
    {
        services.AddHostedService<MostRecent100CacheHostedService>();
        services.AddSingleton<MostRecent100Cache>();
        return services;
    }
}
