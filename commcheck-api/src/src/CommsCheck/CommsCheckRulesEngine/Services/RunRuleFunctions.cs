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
            rulesEngine,
            toCheck,
            (str) => IRuleOutcome.Blocked(methodToLog, str));
    }

    public async Task<IRuleOutcome> RunAllowed(
        string method,
        RulesEngine.RulesEngine rulesEngine,
        CommsCheckItem toCheck)
    {
        return await RunRules(
            "Allow",
            method,
            rulesEngine,
            toCheck,
            (str) => IRuleOutcome.Allowed(method, str));
    }

    private async Task<IRuleOutcome> RunRules(
    string ruleSet,
    string method,
    RulesEngine.RulesEngine rulesEngine,
    CommsCheckItem toCheck,
    Func<string, IRuleOutcome> onSuccess)
    {
        var results = await rulesEngine.ExecuteAllRulesAsync(ruleSet + "-" + method, toCheck);
        CheckForExceptions(results);
        IRuleOutcome rVal = IRuleOutcome.Ignored();

        results.OnSuccess((a) =>
        {
            rVal = onSuccess(a);
        });

        return rVal;
    }

    private void CheckForExceptions(IEnumerable<RulesEngine.Models.RuleResultTree> results)
    {
        foreach (var result in results.Where(x => !string.IsNullOrEmpty(x.ExceptionMessage)))
        {
            _logger.LogError("Failure to run rules with {exception}", result.ExceptionMessage);
        }
    }
}