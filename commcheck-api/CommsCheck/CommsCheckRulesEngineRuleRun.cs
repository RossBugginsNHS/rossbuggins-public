using RulesEngine.Extensions;

public class CommsCheckRulesEngineRuleRun<T>(ILogger<CommsCheckRulesEngineRuleRun<T>> _logger) :
    ICommsCheckRulesEngineRuleRun<T> where T : IContactType
{
    RulesEngine.RulesEngine _rulesEngine;


    public async Task<IRuleOutcome> Run(RulesEngine.RulesEngine rulesEngine, CommsCheckItem toCheck)
    {
        _rulesEngine = rulesEngine;
        return await RunRule(toCheck);
    }

    private async Task<IRuleOutcome> RunRule(CommsCheckItem toCheck)
    {
        var result = await RunRuleWithoutLog(toCheck);
        LogFinalRule(toCheck, result);
        return result;
    }

    private async Task<IRuleOutcome> RunRuleWithoutLog(CommsCheckItem toCheck)
    {
        var currentMethod = GetMethod();

        var explictBlockAllRulesStatus = await RunExplictBlockAll(currentMethod, toCheck);
        if (explictBlockAllRulesStatus.IsBlocked())
            return explictBlockAllRulesStatus;

        var explictBlockMethodRulesStatus = await RunExplictBlock(currentMethod, toCheck);
        if (explictBlockMethodRulesStatus.IsBlocked())
            return explictBlockMethodRulesStatus;

        var allowMethodRulesStatus = await RunAllowed(currentMethod, toCheck);
        if (allowMethodRulesStatus.IsAllowed())
            return allowMethodRulesStatus;

        return IRuleOutcome.Blocked(currentMethod, "Default Block");
    }

    private async Task<IRuleOutcome> RunExplictBlockAll(string method, CommsCheckItem toCheck)
    {
        return await RunExplictBlock("All", method, toCheck);
    }

    private async Task<IRuleOutcome> RunExplictBlock(string method, CommsCheckItem toCheck)
    {
        return await RunExplictBlock(method, method, toCheck);
    }

    private async Task<IRuleOutcome> RunExplictBlock(string methodToCheck, string methodToLog, CommsCheckItem toCheck)
    {
        return await RunRules(
            "ExplicitBlock",
            methodToCheck,
            toCheck,
            (str) => IRuleOutcome.Blocked(methodToLog, str));
    }

    private async Task<IRuleOutcome> RunAllowed(string method, CommsCheckItem toCheck)
    {
        return await RunRules(
            "Allow",
            method,
            toCheck,
            (str) => IRuleOutcome.Allowed(method, str));
    }

    private string GetMethod() => typeof(T).Name;

    private void LogFinalRule(
        CommsCheckItem item,
        IRuleOutcome outcome)
    {
        _logger.LogInformation(
            "Final Check on {T} for item {item} with outcome {outcome}",
            GetMethod(),
            item,
            outcome.ToString());
    }

    private async Task<IRuleOutcome> RunRules(
        string ruleSet,
        string method,
        CommsCheckItem toCheck,
        Func<string, IRuleOutcome> onSuccess)
    {
        var results = await _rulesEngine.ExecuteAllRulesAsync(ruleSet + "-" + method, toCheck);
        //Here can log on each of the rules that was run.

        IRuleOutcome rVal = IRuleOutcome.Ignored();

        results.OnSuccess((a) =>
        {
            rVal = onSuccess(a);
        });

        return rVal;
    }


}
