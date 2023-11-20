namespace CommsCheck;
using RulesEngine;

public static class RulesEngineExtensionMethods
{
    public static IServiceCollection AddRulesEngine(this IServiceCollection services)
    {
        services.AddTransient<RulesEngineRulesLoader>();
        services.AddHostedService<RulesEngineLoaderHostedService>();
        services.AddSingleton<RuleEngineFactory>();
        services.AddSingleton<RulesEngineAndHash>(sp =>
        {
            var rehosted = sp.GetRequiredService<RuleEngineFactory>();
            var re = rehosted.RulesEngine;
            ArgumentNullException.ThrowIfNull(re);
            return re;
        });
        return services;
    }
}
