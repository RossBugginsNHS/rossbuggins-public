namespace CommsCheck;

public record RunRuleResultAndSummaries(IRuleOutcome Outcome, IEnumerable<RuleResultSummary> Summaries);
