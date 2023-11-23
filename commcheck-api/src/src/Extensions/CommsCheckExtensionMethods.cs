namespace CommsCheck;

using System.Threading.Channels;
using MediatR;
using OpenTelemetry.Metrics;

public static class CommsCheckExtensionMethods
{
    public static IServiceCollection AddCommsCheck(this IServiceCollection services, Action<CommsCheckOptions> options)
    {
        services.AddCommsCheck();
        var optionsInstance = new CommsCheckOptions(services);
        options(optionsInstance);

        return services;
    }

    public static IServiceCollection AddCommsCheck(this IServiceCollection services)
    {
        services.AddSingleton<CommsCheckItemFactory>();
        services.AddSingleton(TimeProvider.System);
        services.AddRule100();
        services.AddHostedService<CommsCheckHostedService>();
        services.AddSingleton<RunRuleFunctions>();
        services.AddSingleton<RulesCombinerService>();
        services.AddSingleton<RuleRunMethodResultCacheService>();
        services.AddSingleton(Channel.CreateUnbounded<CommsCheckItemWithId>(new UnboundedChannelOptions() { SingleReader = true }));
        services.AddSingleton(svc => svc.GetRequiredService<Channel<CommsCheckItemWithId>>().Reader);
        services.AddSingleton(svc => svc.GetRequiredService<Channel<CommsCheckItemWithId>>().Writer);
        services.AddMediatR(config =>
        {
            config.MediatorImplementationType = typeof(PublishWithMetricsAndLogging);
            config.RegisterServicesFromAssemblyContaining<CommsCheckOptions>();
            config.AddOpenBehavior(typeof(LoggingCommandsBehavior<,>));
        });
        return services;
    }
}