namespace CommsCheck;

using MediatR;

public class RuleRunMethodResultEvent(
    Guid ruleRunId,
    string method,
    CommsCheckItemWithId toCheck,
    IRuleOutcome outcome) : ICommsCheckEvent
{
    public Guid CommCheckCorrelationId => ruleRunId;
    public Guid RuleRubId => ruleRunId;
    public IRuleOutcome Outcome => outcome;
    public CommsCheckItemWithId ToCheck => toCheck;
    public string Method => method;
}
