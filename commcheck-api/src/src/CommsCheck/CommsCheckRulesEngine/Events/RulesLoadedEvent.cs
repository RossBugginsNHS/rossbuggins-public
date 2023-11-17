namespace CommsCheck;
using MediatR;

public class RulesLoadedEvent(
    Guid commCheckCorrelationId,
    Guid ruleRunId,
    RulesEngine.RulesEngine rulesEngine,
    string method,
    CommsCheckItemWithId toCheck) : ICommsCheckEvent
{
    public Guid CommCheckCorrelationId => commCheckCorrelationId;
    public Guid RuleRunId => ruleRunId;
    public RulesEngine.RulesEngine RulesEngine => rulesEngine;
    public string Method => method;
    public CommsCheckItemWithId ToCheck => toCheck;
}
