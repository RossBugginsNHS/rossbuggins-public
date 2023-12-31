namespace CommsCheck;

using MediatR;

public class RuleResultsCombinedEvent(
    Guid commCheckCorrelationId,
    Guid ruleRunId,
    string ruleHash,
    string method,
    CommsCheckItemWithId toCheck,
    IEnumerable<IRuleOutcome> outcomes,
    IEnumerable<RuleResultSummary> summaries) : ICommsCheckEvent
{
    public Guid CommCheckCorrelationId => commCheckCorrelationId;
    public Guid RuleRunId => ruleRunId;
    public IEnumerable<IRuleOutcome> Outcomes => outcomes;
    public CommsCheckItemWithId ToCheck => toCheck;
    public string Method => method;
    public string RuleHash => ruleHash;

    public IEnumerable<RuleResultSummary> Summaries => summaries;
}
