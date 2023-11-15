namespace CommsCheck;

using MediatR;

public class RuleResultsCombinedEvent(
    Guid commCheckCorrelationId,
    Guid ruleRunId,
    string method,
    CommsCheckItemWithId toCheck,
    IEnumerable<IRuleOutcome> outcomes) : ICommsCheckEvent
{
    public Guid CommCheckCorrelationId => commCheckCorrelationId;
    public Guid RuleRunId => ruleRunId;
    public IEnumerable<IRuleOutcome> Outcomes => outcomes;
    public CommsCheckItemWithId ToCheck => toCheck;
    public string Method => method;
}
