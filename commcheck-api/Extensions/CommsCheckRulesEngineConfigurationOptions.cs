public class CommsCheckRulesEngineConfigurationOptions(IServiceCollection services)
{
    public  const string OptionsName = "RuleEngine";
    public string RulesPath{get;set;} = string.Empty;

    public CommsCheckRulesEngineConfigurationOptions AddContactType<T>() where T: IContactType
    {
        services.AddTransient(typeof(ICommsCheckRulesEngineRuleRun<IContactType>), typeof(CommsCheckRulesEngineRuleRun<T>));   
        return this;
    }
}