public readonly record struct RuleBlocked(string Reason): IRuleOutcome
{
    public bool Equals(IRuleOutcome? other)
    {
        if (other is RuleBlocked o)
            return o == this;
        return false;
    }
}
