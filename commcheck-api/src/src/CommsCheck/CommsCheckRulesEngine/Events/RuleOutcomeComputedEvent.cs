namespace CommsCheck;

using MediatR;

public class RuleOutcomeComputedEvent(
    Guid commCheckCorrelationId,
    Guid ruleRunId,
    string ruleHash,
    string method,
    CommsCheckItemWithId toCheck,
    IRuleOutcome outcome) : ICommsCheckEvent
{
    public Guid CommCheckCorrelationId => commCheckCorrelationId;
    public Guid RuleRubId => ruleRunId;
    public IRuleOutcome Outcome => outcome;
    public CommsCheckItemWithId ToCheck => toCheck;
    public string Method => method;
    public string RuleHash => ruleHash;
}
