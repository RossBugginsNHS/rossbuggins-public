namespace CommsCheck;

using RulesEngine.Extensions;
using RulesEngine.Models;

public class RunRuleFunctions(ILogger<RunRuleFunctions> _logger)
{
    public const string MethodAll = "All";
    public const string RuleSetExplictBlock = "ExplicitBlock";
    public const string RuleSetAllow = "Allow";
    public const string DefaultBlock = "DefaultBlock";
    public const string RuleParamaterName = "item";

    public async Task<RunRuleResultAndSummaries> RunExplictBlockAll(
        string method,
        RulesEngine.RulesEngine rulesEngine,
        CommsCheckItem toCheck)
    {
        return await RunExplictBlock(
            RunRuleFunctions.MethodAll,
            method,
            rulesEngine,
            toCheck);
    }

    public async Task<RunRuleResultAndSummaries> RunExplictBlock(
        string method,
        RulesEngine.RulesEngine rulesEngine,
        CommsCheckItem toCheck)
    {
        return await RunExplictBlock(
            method, 
            method, 
            rulesEngine, 
            toCheck);
    }

    public async Task<RunRuleResultAndSummaries> RunAllowed(
        string method,
        string methodToLog,
        RulesEngine.RulesEngine rulesEngine,
        CommsCheckItem toCheck)
    {
        return await RunRules(
            RunRuleFunctions.RuleSetAllow,
            method,
            methodToLog,
            rulesEngine,
            toCheck,
            (str) => IRuleOutcome.Allowed(
                RunRuleFunctions.RuleSetAllow,
                method,
                str));
    }

    private async Task<RunRuleResultAndSummaries> RunExplictBlock(
        string methodToCheck,
        string methodToLog,
        RulesEngine.RulesEngine rulesEngine,
        CommsCheckItem toCheck)
    {
        return await RunRules(
            RunRuleFunctions.RuleSetExplictBlock,
            methodToCheck,
            methodToLog,
            rulesEngine,
            toCheck,
            (str) => IRuleOutcome.Blocked(
                RunRuleFunctions.RuleSetExplictBlock,
                methodToLog,
                str));
    }

    private async Task<RunRuleResultAndSummaries> RunRules(
    string ruleSet,
    string method,
    string methodToLog,
    RulesEngine.RulesEngine rulesEngine,
    CommsCheckItem toCheck,
    Func<string, IRuleOutcome> onSuccess)
    {
        var results = await ExecuteRules(ruleSet, method, toCheck, rulesEngine);
        var summaries = Summaries(ruleSet, method, methodToLog, results).ToList();
        var ruleOutcome = IgnoreOrBlockOrSuccessResponseForRules(
            results,
            ruleSet,
            methodToLog,
            onSuccess);

        return new RunRuleResultAndSummaries(ruleOutcome, summaries);
    }

    private async Task<List<RuleResultTree>> ExecuteRules(
        string ruleSet, 
        string method, 
        CommsCheckItem toCheck,
        RulesEngine.RulesEngine rulesEngine) =>
            await rulesEngine.ExecuteAllRulesAsync(
                RuleSetFull(ruleSet, method), 
                MakeRuleParameter(toCheck));
            
    private  RuleParameter MakeRuleParameter(CommsCheckItem toCheck) => 
        new RuleParameter(RuleParamaterName, toCheck);

    private string RuleSetFull(string ruleSet, string method) => 
        ruleSet + "-" + method;

    private IRuleOutcome IgnoreOrBlockOrSuccessResponseForRules(
        List<RulesEngine.Models.RuleResultTree> results,
         string ruleSet,
          string methodToLog,
        Func<string, IRuleOutcome> onSuccess)
    {
        var errored = CheckForExceptions(results);
        var rVal = IgnoreOrBlock(ruleSet, methodToLog, errored);

        results.OnSuccess((outComeMessage) =>
        {
            rVal = BlockedOrSuccess(rVal, outComeMessage, onSuccess);
        });

        return rVal;
    }

    private IRuleOutcome BlockedOrSuccess(
        IRuleOutcome rVal,
        string outComeMessage,
        Func<string, IRuleOutcome> onSuccess) =>

            rVal switch
            {
                RuleBlocked => rVal,
                _ => onSuccess(outComeMessage)
            };


    private IRuleOutcome IgnoreOrBlock(string ruleSet, string methodToLog, RuleException errored) =>
        errored.Exception switch
        {
            true => IRuleOutcome.Blocked(ruleSet, methodToLog, ErrorString(errored)),
            _ => IRuleOutcome.Ignored()
        };

    private string ErrorString(RuleException errored) =>
        $"Error running rules {string.Join(", ", errored.ExceptionStrings)}";

    private RuleException CheckForExceptions(IEnumerable<RulesEngine.Models.RuleResultTree> results)
    {
        var error = RuleException.Empty;
        foreach (var result in ExceptionResults(results))
        {
            error = BuildRuleException(error, result);
        }
        return error;
    }

    private IEnumerable<string> ExceptionResults(IEnumerable<RulesEngine.Models.RuleResultTree> results) =>
        results
            .Where(x => !string.IsNullOrEmpty(x.ExceptionMessage))
            .Select(x => x.ExceptionMessage);

    private RuleException BuildRuleException(RuleException error, string result)
    {
        _logger.LogError("Failure to run rules with {exception}", result);
        return error with
        {
            Exception = true,
            ExceptionStrings = error.ExceptionStrings.Append(result).ToArray()
        };
    }

    private IEnumerable<RuleResultSummary> Summaries(
        string ruleSet,
        string method,
        string methodToLog,
        IEnumerable<RulesEngine.Models.RuleResultTree> results) =>
            results.Select(result => FromResult(ruleSet, method, methodToLog, result));

    private RuleResultSummary FromResult(
        string ruleSet,
        string method,
        string methodToLog,
        RulesEngine.Models.RuleResultTree result) =>
            new RuleResultSummary(
                ruleSet,
                method,
                methodToLog,
                result.Rule.Enabled,
                result.IsSuccess,
                result.Rule.RuleName,
                result.Rule.Expression,
                result.Rule.SuccessEvent,
                result.Rule.ErrorMessage,
                result.ExceptionMessage);
}