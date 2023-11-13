public interface ICommsCheckRulesEngineRuleRun<out T> where T : IContactType
{
    Task Run(RulesEngine.RulesEngine rulesEngine, CommsCheckItemWithId toCheck);
}
