namespace CommsCheck;
public readonly record struct RuleBlocked(string Method, string Reason): IRuleOutcome
{
    public bool Equals(IRuleOutcome? other)
    {
        if (other is RuleBlocked o)
            return o == this;
        return false;
    }
}
