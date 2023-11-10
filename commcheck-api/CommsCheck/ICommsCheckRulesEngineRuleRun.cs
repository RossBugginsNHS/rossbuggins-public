public interface ICommsCheckRulesEngineRuleRun<out T> where T : IContactType
{
    Task<IRuleOutcome> Run(RulesEngine.RulesEngine rulesEngine, CommsCheckItem toCheck);
}
