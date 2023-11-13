using MediatR;

public class RuleResultsCombinedEvent(
    Guid ruleRunId,
    string method,
    CommsCheckItemWithId toCheck,
    IEnumerable<IRuleOutcome> outcomes) : INotification
{
    public Guid RuleRubId => ruleRunId;
    public IEnumerable<IRuleOutcome> Outcomes => outcomes;
    public CommsCheckItemWithId ToCheck => toCheck;
    public string Method => method;
}
