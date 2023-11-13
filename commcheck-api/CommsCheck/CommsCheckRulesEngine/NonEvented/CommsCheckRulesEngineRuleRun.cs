using MediatR;
using Microsoft.AspNetCore.Rewrite;

[Obsolete]
public class CommsCheckRulesEngineRuleRun<T>(
    ILogger<CommsCheckRulesEngineRuleRun<T>> _logger) :
    ICommsCheckRulesEngineRuleRun<T> where T : IContactType
{
    RulesEngine.RulesEngine _rulesEngine;


    public async Task Run(RulesEngine.RulesEngine rulesEngine, CommsCheckItemWithId toCheck)
    {
        throw new NotImplementedException();

        _rulesEngine = rulesEngine;
        //return await RunRule(toCheck.Item);
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

        var explictBlockAllRulesStatus = await RunRuleFunctions.RunExplictBlockAll(currentMethod, _rulesEngine, toCheck);
        if (explictBlockAllRulesStatus.IsBlocked())
            return explictBlockAllRulesStatus;

        var explictBlockMethodRulesStatus = await RunRuleFunctions.RunExplictBlock(currentMethod, _rulesEngine, toCheck);
        if (explictBlockMethodRulesStatus.IsBlocked())
            return explictBlockMethodRulesStatus;

        var allowMethodRulesStatus = await RunRuleFunctions.RunAllowed(currentMethod, _rulesEngine, toCheck);
        if (allowMethodRulesStatus.IsAllowed())
            return allowMethodRulesStatus;

        return IRuleOutcome.Blocked(currentMethod, "Default Block");
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


}
