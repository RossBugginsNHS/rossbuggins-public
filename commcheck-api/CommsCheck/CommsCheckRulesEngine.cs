using Microsoft.Extensions.Options;
using RulesEngine.Extensions;
using RulesEngine.Models;

public class CommsCheckRulesEngine : ICommCheck
{
    private readonly RulesEngine.RulesEngine _rulesEngine;
    private readonly CommsCheckItemSha _sha;
    private readonly ILogger<CommsCheckRulesEngine> _logger;
    private readonly IOptions<CommsCheckRulesEngineOptions> _options;

    public CommsCheckRulesEngine(
        CommsCheckItemSha sha,
         ILogger<CommsCheckRulesEngine> logger,
         IOptions<CommsCheckRulesEngineOptions> options)
    {
        _sha = sha;
        _logger = logger;
        _options = options;
        var fileData = File.ReadAllText(_options.Value.JsonPath);
        var workflow = System.Text.Json.JsonSerializer.Deserialize<List<Workflow>>(fileData);
        _rulesEngine = new RulesEngine.RulesEngine(workflow.ToArray());
    }

    public async Task<CommsCheckAnswer> Check(CommsCheckItemWithId toCheck)
    {
       // var id = await _sha.GetSha(toCheck, "Getting id in Rules engine Check.");
        var sms = await RunRule<Sms>(toCheck.Item);
        var app = await RunRule<App>(toCheck.Item);
        var email = await RunRule<Email>(toCheck.Item);
        var postal = await RunRule<Postal>(toCheck.Item);

        return new CommsCheckAnswer(
            toCheck.Id,
            toCheck.Item.ToString(),
            app,
            email,
            sms,
            postal);
    }

    private async Task<IRuleOutcome> RunRule<T>(CommsCheckItem toCheck) where T : IContactType
    {
        var result = await RunRuleWithoutLog<T>(toCheck);
        LogFinalRule<T>(toCheck, result);
        return result;
    }

    private async Task<IRuleOutcome> RunRuleWithoutLog<T>(CommsCheckItem toCheck) where T : IContactType
    {
        var explictBlockAllRulesStatus = await RunExplictBlock(toCheck);
        if (explictBlockAllRulesStatus.IsBlocked())
            return explictBlockAllRulesStatus;

        var explictBlockMethodRulesStatus = await RunExplictBlock<T>(toCheck);
        if (explictBlockMethodRulesStatus.IsBlocked())
            return explictBlockMethodRulesStatus;

        var allowMethodRulesStatus = await RunAllowed<T>(toCheck);
        if (allowMethodRulesStatus.IsAllowed())
            return allowMethodRulesStatus;

        return IRuleOutcome.Blocked("Default Block");
    }

    private async Task<IRuleOutcome> RunExplictBlock(CommsCheckItem toCheck)
    {
        return await RunExplictBlock(toCheck, "All");
    }

    private async Task<IRuleOutcome> RunExplictBlock<T>(CommsCheckItem toCheck) where T : IContactType
    {
         var method = GetMethod<T>();
        return await RunExplictBlock(toCheck, method);
    }

    private async Task<IRuleOutcome> RunExplictBlock(CommsCheckItem toCheck, string method)
    {
        return await RunRules(
            "ExplicitBlock",
            method,
            toCheck,
            (str) => IRuleOutcome.Blocked(str));
    }

    private async Task<IRuleOutcome> RunAllowed<T>(CommsCheckItem toCheck) where T : IContactType
    {
        var method = GetMethod<T>();
        return await RunRules(
            "Allow",
            method,
            toCheck,
            (str) => IRuleOutcome.Allowed(str));
    }

    private string GetMethod<T>() where T: IContactType => typeof(T).Name;

    private void LogFinalRule<T>(
        CommsCheckItem item,
        IRuleOutcome outcome) where T : IContactType
    {
        _logger.LogInformation(
            "Final Check on {T} for item {item} with outcome {outcome}",
            typeof(T).Name,
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
