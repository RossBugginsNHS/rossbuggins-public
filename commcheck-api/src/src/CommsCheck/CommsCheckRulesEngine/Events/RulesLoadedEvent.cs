namespace CommsCheck;
using MediatR;

public class RulesLoadedEvent(
    Guid commCheckCorrelationId,
    Guid ruleRunId,
    string method,
    CommsCheckItemWithId toCheck) : ICommsCheckEvent
{
    public Guid CommCheckCorrelationId => commCheckCorrelationId;
    public Guid RuleRunId => ruleRunId;
    public string Method => method;
    public CommsCheckItemWithId ToCheck => toCheck;
}
