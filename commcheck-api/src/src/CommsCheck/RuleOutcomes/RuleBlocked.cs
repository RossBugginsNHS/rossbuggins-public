namespace CommsCheck;
public readonly record struct RuleBlocked(string RuleSet, string Method, string Reason): IRuleOutcome
{
    public bool Equals(IRuleOutcome? other)
    {
        if (other is RuleBlocked o)
            return o == this;
        return false;
    }
    public bool IsAllowed => false;
    public bool IsBlocked => true;
    public string OutcomeDescription => "Blocked";
}

