namespace CommsCheck;

using System.Diagnostics;
using MediatR;

public class RulesRunCompleteEvent(
    Guid commCheckCorrelationId,
    Guid ruleRunId,
    IRuleOutcome outcome,
    string method,
    CommsCheckItemWithId toCheck
) : ICommsCheckEvent
{
    public Guid CommCheckCorrelationId => commCheckCorrelationId;
    public Guid RuleRunId => ruleRunId;
    public IRuleOutcome Outcome => outcome;

    public string Method => method;
    public CommsCheckItemWithId ToCheck => toCheck;
}
