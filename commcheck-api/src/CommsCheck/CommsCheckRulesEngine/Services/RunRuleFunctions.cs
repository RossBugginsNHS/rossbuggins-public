namespace CommsCheck;

using RulesEngine.Extensions;

public static class RunRuleFunctions
{

    public static async Task<IRuleOutcome> RunExplictBlockAll(
        string method,
        RulesEngine.RulesEngine rulesEngine,
        CommsCheckItem toCheck)
    {
        return await RunExplictBlock("All", method, rulesEngine, toCheck);
    }

    public static async Task<IRuleOutcome> RunExplictBlock(
        string method,
        RulesEngine.RulesEngine rulesEngine,
        CommsCheckItem toCheck)
    {
        return await RunExplictBlock(method, method, rulesEngine, toCheck);
    }

    private static async Task<IRuleOutcome> RunExplictBlock(
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

    public static async Task<IRuleOutcome> RunAllowed(
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

    private static async Task<IRuleOutcome> RunRules(
    string ruleSet,
    string method,
    RulesEngine.RulesEngine rulesEngine,
    CommsCheckItem toCheck,
    Func<string, IRuleOutcome> onSuccess)
    {
        var results = await rulesEngine.ExecuteAllRulesAsync(ruleSet + "-" + method, toCheck);
        //Here can log on each of the rules that was run.

        IRuleOutcome rVal = IRuleOutcome.Ignored();

        results.OnSuccess((a) =>
        {
            rVal = onSuccess(a);
        });

        return rVal;
    }
}