using System.Threading.Channels;

public static class CommsCheckExtensionMethods
{
    public static IServiceCollection AddCommsCheck(this IServiceCollection services, Action<CommsCheckOptions> options)
    {
        services.AddCommsCheck();
        var optionsInstance = new CommsCheckOptions(services);
        options(optionsInstance);
        services.AddSingleton(_sp => new CommsCheckItemSha( 
            _sp.GetService<ILogger<CommsCheckItemSha>>(), 
            optionsInstance.ShaKey));
        return services;
    }

    public static IServiceCollection AddCommsCheck(this IServiceCollection services)
    {
       
      
        services.AddHostedService<CommsCheckHostedService>();
        services.AddSingleton(Channel.CreateUnbounded<CommsCheckItemWithId>(new UnboundedChannelOptions() { SingleReader = true }));
        services.AddSingleton(svc => svc.GetRequiredService<Channel<CommsCheckItemWithId>>().Reader);
        services.AddSingleton(svc => svc.GetRequiredService<Channel<CommsCheckItemWithId>>().Writer);
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblyContaining<CommsCheckOptions>();
        });
        return services;
    }
}