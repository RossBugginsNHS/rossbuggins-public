namespace CommsCheck;
public readonly record struct RuleAllowed(string RuleSet, string Method, string Reason) : IRuleOutcome
{
    public bool Equals(IRuleOutcome? other)
    {
        if (other is RuleAllowed o)
            return o == this;
        return false;
    }

    public bool IsAllowed => true;
    public bool IsBlocked => false;
    public string OutcomeDescription => "Allowed";
}

