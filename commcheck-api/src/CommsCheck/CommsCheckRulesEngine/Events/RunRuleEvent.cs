namespace CommsCheck;

using MediatR;

public class RunRuleEvent(
    Guid commCheckCorrelationId, 
    ICommsCheckRulesEngineRuleRun<IContactType> rule,
    RulesEngine.RulesEngine rules,
    CommsCheckItemWithId toCheck)
    : ICommsCheckEvent
{
    public Guid CommCheckCorrelationId => commCheckCorrelationId;
    public ICommsCheckRulesEngineRuleRun<IContactType> Rule => rule;
    public RulesEngine.RulesEngine Rules => rules;
    public CommsCheckItemWithId ToCheck => toCheck;
}
