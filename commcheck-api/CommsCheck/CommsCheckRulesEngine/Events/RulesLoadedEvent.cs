using MediatR;

public class RulesLoadedEvent(
    Guid ruleRunId,
    RulesEngine.RulesEngine rulesEngine,
    string method,
    CommsCheckItemWithId toCheck) : INotification
{
    public Guid RuleRunId => ruleRunId;
    public RulesEngine.RulesEngine RulesEngine => rulesEngine;
    public string Method => method;
    public CommsCheckItemWithId ToCheck => toCheck;
}
