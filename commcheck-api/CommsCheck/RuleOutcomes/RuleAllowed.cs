public readonly record struct RuleAllowed(string Reason) : IRuleOutcome
{
    public bool Equals(IRuleOutcome? other)
    {
        if (other is RuleAllowed o)
            return o == this;
        return false;
    }
}

