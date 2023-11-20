namespace CommsCheck;

using RulesEngine.Extensions;

public class RunRuleFunctions(ILogger<RunRuleFunctions> _logger)
{

    public async Task<IRuleOutcome> RunExplictBlockAll(
        string method,
        RulesEngine.RulesEngine rulesEngine,
        CommsCheckItem toCheck)
    {
        return await RunExplictBlock("All", method, rulesEngine, toCheck);
    }

    public async Task<IRuleOutcome> RunExplictBlock(
        string method,
        RulesEngine.RulesEngine rulesEngine,
        CommsCheckItem toCheck)
    {
        return await RunExplictBlock(method, method, rulesEngine, toCheck);
    }

    private async Task<IRuleOutcome> RunExplictBlock(
        string methodToCheck,
        string methodToLog,
        RulesEngine.RulesEngine rulesEngine,
        CommsCheckItem toCheck)
    {
        return await RunRules(
            "ExplicitBlock",
            methodToCheck,
            methodToLog,
            rulesEngine,
            toCheck,
            (str) => IRuleOutcome.Blocked(methodToLog, str));
    }

    public async Task<IRuleOutcome> RunAllowed(
        string method,
        string methodToLog,
        RulesEngine.RulesEngine rulesEngine,
        CommsCheckItem toCheck)
    {
        return await RunRules(
            "Allow",
            method,
            methodToLog,
            rulesEngine,
            toCheck,
            (str) => IRuleOutcome.Allowed(method, str));
    }

    private async Task<IRuleOutcome> RunRules(
    string ruleSet,
    string method,
    string methodToLog,
    RulesEngine.RulesEngine rulesEngine,
    CommsCheckItem toCheck,
    Func<string, IRuleOutcome> onSuccess)
    {
        var results = await rulesEngine.ExecuteAllRulesAsync(ruleSet + "-" + method, toCheck);
        var errored = CheckForExceptions(results);
        if(errored.Exception)
        {
            return IRuleOutcome.Blocked(methodToLog, $"Error running rules {string.Join(", ", errored.ExceptionStrings)}");
        }
        IRuleOutcome rVal = IRuleOutcome.Ignored();

        results.OnSuccess((a) =>
        {
            rVal = onSuccess(a);
        });

        return rVal;
    }

    private RuleException CheckForExceptions(IEnumerable<RulesEngine.Models.RuleResultTree> results)
    {
        var error = new RuleException(false, Array.Empty<string>());
        foreach (var result in results
            .Where(x => !string.IsNullOrEmpty(x.ExceptionMessage))
            .Select(x=> x.ExceptionMessage))
        {
            _logger.LogError("Failure to run rules with {exception}", result);
            error = error with {
                Exception = true, 
                ExceptionStrings = error.ExceptionStrings.Append(result).ToArray()};
        
        }

        return error;
    }
}

public readonly record struct RuleException(bool Exception, string[] ExceptionStrings)
{
}