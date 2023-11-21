namespace CommsCheck;
public readonly record struct RuleIgnored(string RuleSet, string Method, string Reason) :IRuleOutcome
{
    public bool Equals(IRuleOutcome? other)
    {
        if (other is RuleIgnored o)
            return o == this;
        return false;
    }

    public bool IsAllowed => false;
    public bool IsBlocked => false;
    public string OutcomeDescription => "Ignored";
}
