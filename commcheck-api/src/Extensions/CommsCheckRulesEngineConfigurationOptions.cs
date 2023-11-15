namespace CommsCheck;
public class CommsCheckRulesEngineConfigurationOptions
{
    private readonly IServiceCollection _services;
    public  CommsCheckRulesEngineConfigurationOptions(IServiceCollection services)
    {
        _services = services;
    }

    public  const string OptionsName = "RuleEngine";
    public string RulesPath{get;set;} = string.Empty;

    public CommsCheckRulesEngineConfigurationOptions AddContactType<T, U>() 
        where T: class, IContactType
        where U: class, ICommsCheckRulesEngineRuleRun<T>
    {

        _services.AddTransient(
            typeof(ICommsCheckRulesEngineRuleRun<IContactType>), 
            typeof(U));   
        return this;
    }

    public CommsCheckRulesEngineConfigurationOptions AddContactType<T>() where T: class, IContactType, new()
    {
        _services.AddTransient(typeof(ICommsCheckRulesEngineRuleRun<IContactType>), typeof(CommsCheckRulesEngineRuleRunEvents<T>));   
        return this;
    }
}