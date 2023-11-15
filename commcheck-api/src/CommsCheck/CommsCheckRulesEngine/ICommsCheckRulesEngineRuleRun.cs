namespace CommsCheck;

public interface ICommsCheckRulesEngineRuleRun<out T> where T : IContactType
{
    T Value {get;}
    Task Run(RulesEngine.RulesEngine rulesEngine, CommsCheckItemWithId toCheck);
}
