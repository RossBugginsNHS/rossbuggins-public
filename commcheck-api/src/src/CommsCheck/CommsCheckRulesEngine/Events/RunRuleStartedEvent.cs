namespace CommsCheck;

using MediatR;

public class RunRuleStartedEvent(
    Guid commCheckCorrelationId, 
    ICommsCheckRulesEngineRuleRun<IContactType> rule,
    CommsCheckItemWithId toCheck)
    : ICommsCheckEvent
{
    public Guid CommCheckCorrelationId => commCheckCorrelationId;
    public ICommsCheckRulesEngineRuleRun<IContactType> Rule => rule;
    public CommsCheckItemWithId ToCheck => toCheck;
}
