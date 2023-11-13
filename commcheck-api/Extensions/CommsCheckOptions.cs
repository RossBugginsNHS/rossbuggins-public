using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.ObjectPool;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

public class CommsCheckOptions(IServiceCollection services)
{
    public byte[] ShaKey { get; set; } = new byte[64];


    public CommsCheckOptions AddJsonConfig()
    {
        //https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.httpjsonserviceextensions.configurehttpjsonoptions?view=aspnetcore-7.0
        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.WriteIndented = true;
            options.SerializerOptions.IncludeFields = true;
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        services.Configure<JsonOptions>(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        return this;
    }

    public CommsCheckOptions AddDistriubtedCache()
    {
        services.AddDistributedMemoryCache();
        return this;
    }

    public CommsCheckOptions AddMetrics()
    {
        services.AddOpenTelemetry()

        .WithTracing(builder => builder
            .AddAspNetCoreInstrumentation()
           // .AddConsoleExporter()
            )
        .WithMetrics(
            builder => builder
            .AddMeter("NHS.CommChecker.HashWrapperObjectPoolPolicy")
            .AddMeter("NHS.CommChecker.CheckCommsCommandHandler")
            .AddMeter("NHS.CommChecker.CommsCheckHostedService")
            .AddMeter("NHS.CommChecker.CheckCommsDirectCommandHandler")     
            .AddPrometheusExporter()
            .AddAspNetCoreInstrumentation()
            //.AddConsoleExporter()
            );

        return this;
    }

    public CommsCheckOptions AddNativeRules(Action<CommsCheckNativeRulesOptions> options)
    {
        services.AddTransient<ICommCheck, CommCheckNativeChecks>();
        var optionsInstance = new CommsCheckNativeRulesOptions(services);
        options(optionsInstance);
        return this;
    }

    public CommsCheckOptions AddRulesEngineRules(
        IConfiguration config,
        Action<CommsCheckRulesEngineConfigurationOptions> options)
    {
        return this.AddRulesEngineRules<CommsCheckRulesEngine>(config, options);
    }

    public CommsCheckOptions AddRulesEngineRules<T>(
        IConfiguration config,
        Action<CommsCheckRulesEngineConfigurationOptions> options) where T : class, ICommCheck
    {
        services.AddTransient<ICommCheck, T>();
        var optionsInstance = new CommsCheckRulesEngineConfigurationOptions(services);


        config.GetSection(CommsCheckRulesEngineConfigurationOptions.OptionsName).Bind(optionsInstance);

        options(optionsInstance);

        services.Configure<CommsCheckRulesEngineOptions>(x =>
        {
            x.JsonPath = optionsInstance.RulesPath;
        });

        return this;
    }

    public CommsCheckOptions AddShaKey(string key)
    {
        ShaKey = Encoding.UTF8.GetBytes(key);

        services.Configure<HashWrapperOptions>(x =>
        {
            x.HashKey = ShaKey;
        });

        //services.AddSingleton<CommsCheckItemSha>();
        services.TryAddSingleton<HashWrapperObjectPoolPolicy>();
        services.TryAddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
        services.TryAddSingleton<ObjectPool<HashWrapper>>(serviceProvider =>
        {
            var provider = serviceProvider.GetRequiredService<ObjectPoolProvider>();
            var policy = serviceProvider.GetRequiredService<HashWrapperObjectPoolPolicy>();
            return provider.Create(policy);
        });

        return this;
    }
}