namespace CommsCheck;

using MediatR;

public class RuleRunMethodResultEvent(
    Guid commCheckCorrelationId,
    Guid ruleRunId,
    string method,
    CommsCheckItemWithId toCheck,
    IRuleOutcome outcome) : ICommsCheckEvent
{
    public Guid CommCheckCorrelationId => commCheckCorrelationId;
    public Guid RuleRubId => ruleRunId;
    public IRuleOutcome Outcome => outcome;
    public CommsCheckItemWithId ToCheck => toCheck;
    public string Method => method;
}
