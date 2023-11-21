namespace CommsCheck;

using System.Diagnostics;
using MediatR;

public class RulesRunCompletedEvent(
    Guid commCheckCorrelationId,
    Guid ruleRunId,
    string rulesHash,
    IRuleOutcome outcome,
    string method,
    CommsCheckItemWithId toCheck,
    List<RuleResultSummary> ruleSummaries
) : ICommsCheckEvent
{
    public Guid CommCheckCorrelationId => commCheckCorrelationId;
    public Guid RuleRunId => ruleRunId;
    public IRuleOutcome Outcome => outcome;
    public string Method => method;
    public CommsCheckItemWithId ToCheck => toCheck;
    public string RulesHash =>rulesHash; 

    public List<RuleResultSummary> RuleSummaries => ruleSummaries;
}
