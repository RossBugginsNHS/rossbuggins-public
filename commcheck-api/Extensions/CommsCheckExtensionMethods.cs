using System.Threading.Channels;

public static class CommsCheckExtensionMethods
{
    public static IServiceCollection AddCommsCheck(this IServiceCollection services, Action<CommsCheckOptions> options)
    {
        services.AddCommsCheck();
        var optionsInstance = new CommsCheckOptions(services);
        options(optionsInstance);
        services.AddSingleton(_sp => new CommsCheckItemSha(optionsInstance.ShaKey));
        return services;
    }

    public static IServiceCollection AddCommsCheck(this IServiceCollection services)
    {
       
      
        services.AddHostedService<CommsCheckHostedService>();
        services.AddSingleton(Channel.CreateUnbounded<CommsCheckItem>(new UnboundedChannelOptions() { SingleReader = true }));
        services.AddSingleton(svc => svc.GetRequiredService<Channel<CommsCheckItem>>().Reader);
        services.AddSingleton(svc => svc.GetRequiredService<Channel<CommsCheckItem>>().Writer);
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblyContaining<CommsCheckOptions>();
        });
        return services;
    }
}