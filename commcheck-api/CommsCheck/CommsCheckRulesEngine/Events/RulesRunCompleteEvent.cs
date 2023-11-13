using MediatR;

public class RulesRunCompleteEvent(
    Guid ruleRunId,
    IRuleOutcome outcome,
    string method,
    CommsCheckItemWithId toCheck
) : INotification
{
    public Guid RuleRunId => ruleRunId;
    public IRuleOutcome Outcome => outcome;

    public string Method => method;
    public CommsCheckItemWithId ToCheck => toCheck;
}
