namespace CommsCheck;

public readonly record struct RuleResultSummary(
    string RuleSet,
    string Method,
    string MethodToLog,
    bool Enabled,
    bool Success, 
    string RuleName, 
    string RuleExpression,
    string SuccessEvent,
    string ErrorMessage,
    string ExceptionMessage);