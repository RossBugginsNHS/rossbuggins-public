public readonly record struct RuleIgnored(string Method, string Reason) :IRuleOutcome
{
    public bool Equals(IRuleOutcome? other)
    {
        if (other is RuleIgnored o)
            return o == this;
        return false;
    }
}
