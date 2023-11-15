namespace CommsCheck;
public class CommsCheckRulesEngineConfigurationOptions(IServiceCollection services)
{
    public  const string OptionsName = "RuleEngine";
    public string RulesPath{get;set;} = string.Empty;

    public CommsCheckRulesEngineConfigurationOptions AddContactType<T, U>() 
        where T: class, IContactType
        where U: class, ICommsCheckRulesEngineRuleRun<T>
    {

        services.AddTransient(
            typeof(ICommsCheckRulesEngineRuleRun<IContactType>), 
            typeof(U));   
        return this;
    }

    public CommsCheckRulesEngineConfigurationOptions AddContactType<T>() where T: IContactType
    {
        services.AddTransient(typeof(ICommsCheckRulesEngineRuleRun<IContactType>), typeof(CommsCheckRulesEngineRuleRunEvents<T>));   
        return this;
    }
}