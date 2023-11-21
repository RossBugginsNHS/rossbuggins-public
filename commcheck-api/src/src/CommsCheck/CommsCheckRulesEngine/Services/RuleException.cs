namespace CommsCheck;

public readonly record struct RuleException(bool Exception, string[] ExceptionStrings)
{
    public static RuleException Empty => new RuleException(false, Array.Empty<string>());
}