namespace CommsCheck;

public interface ICommsCheckRulesEngineRuleRun<out T> where T : IContactType
{
    T Value {get;}
    Task Run(Guid commCheckCorrelationId, RulesEngine.RulesEngine rulesEngine, CommsCheckItemWithId toCheck);
}
