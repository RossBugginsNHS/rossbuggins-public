using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

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
        services.AddTransient<ICommCheck, CommsCheckRulesEngine>();
        var optionsInstance = new CommsCheckRulesEngineConfigurationOptions();
        

        config.GetSection(CommsCheckRulesEngineConfigurationOptions.OptionsName).Bind(optionsInstance);

        options(optionsInstance);

        services.Configure<CommsCheckRulesEngineOptions>(x=>
        {
            x.JsonPath = optionsInstance.RulesPath;
        });

        return this;
    }

    public CommsCheckOptions AddShaKey(string key)
    {
        ShaKey = Encoding.UTF8.GetBytes(key);
        return this;
    }
}
