using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RulesEngine;

[Obsolete("Use CommsCheckRulesEngine instead.", false)]
public class CommCheckNativeChecks(
    IServiceProvider _services,
    ILogger<CommCheckNativeChecks> _logger) : ICommCheck
{
    public async Task<CommsCheckAnswer> Check(CommsCheckItemWithId item)
    {
        await Task.Yield(); //lazy
        var str = item.Item.ToString();
        var app = CheckRules<App>(item.Item);
        var sms = CheckRules<Sms>(item.Item);
        var email = CheckRules<Email>(item.Item);
        var postal = CheckRules<Postal>(item.Item);
        return new CommsCheckAnswer(item.Id, str, app, email, sms, postal);
    }

    private IRuleOutcome CheckRules<T>(CommsCheckItem item) where T : IContactType
    {
        var ruleCheck = GetRuleChecks<T>(item);
        LogFinalRule<T>(item, ruleCheck);
        return ruleCheck;
    }
    
    private IRuleOutcome GetRuleChecks<T>(CommsCheckItem item) where T : IContactType
    {
        var explicityBlock = ExplicitBlock<T>(item);
        if (explicityBlock.IsBlocked())
            return explicityBlock;

        var allow = ChannelAllow<T>(item);
        if (allow.IsAllowed())
            return allow;

        return IRuleOutcome.Blocked(typeof(T).Name, "Default block due to no rules found.");
    }

    private IRuleOutcome ExplicitBlock<T>(CommsCheckItem item) where T : IContactType
    {
        var generalBlock = AnyChannelExplicitBlock(item);
        if (generalBlock.IsBlocked())
            return generalBlock;

        var channelSpecific = ChannelSpecificExplicitBlock<T>(item);
        return channelSpecific;
    }

    private IRuleOutcome ChannelAllow<T>(CommsCheckItem item) where T : IContactType
    {
        return RunSpecificRules<T>(item, (rule, itm) => rule.Allowed(typeof(T).Name, item));
    }

    private IRuleOutcome ChannelSpecificExplicitBlock<T>(CommsCheckItem item) where T : IContactType
    {
        return RunSpecificRules<T>(item, (rule, itm) => rule.Block(typeof(T).Name, item));
    }

    private IEnumerable<ICommCheckRule> AllChannelRules()
    {
        return _services.GetServices<ICommCheckRule>();
    }

    public IEnumerable<ICommCheckRule<T>> ChannelTRules<T>() where T : IContactType
    {
        return _services.GetServices<ICommCheckRule<T>>();
    }

    private IRuleOutcome AnyChannelExplicitBlock(CommsCheckItem item)
    {
        var rules = AllChannelRules();
        return RunRules(rules.Select(r => r.Block("UnknownMethod", item)));
    }

    private IRuleOutcome RunSpecificRules<T>(CommsCheckItem item, Func<ICommCheckRule<T>, CommsCheckItem, IRuleOutcome> rule) where T : IContactType
    {
        var channelSpecificRules = ChannelTRules<T>();
        return RunRules(channelSpecificRules.Select(r => rule(r, item)));
    }

    private IRuleOutcome RunRules(IEnumerable<IRuleOutcome> ruleRun)
    {
        var outcomes = ExecuteRules(ruleRun);
        LogRules(outcomes);
        return CheckRules(outcomes);
    }

    private IList<IRuleOutcome> ExecuteRules(IEnumerable<IRuleOutcome> ruleRun)
    {
        return ruleRun.ToList();
    }

    private IRuleOutcome CheckRules(IEnumerable<IRuleOutcome> outcomes)
    {
        if (outcomes.Any(x => x.IsBlocked()))
        {
            var blockedReasons = outcomes.Where(x => x.IsBlocked()).Select(x => x.Reason).ToList();
            return IRuleOutcome.Blocked("UnknownMethod", String.Join(", ", blockedReasons));
        }

        if (outcomes.Any(x => x.IsAllowed()))
        {
            var allowedReasons = outcomes.Where(x => x.IsAllowed()).Select(x => x.Reason).ToList();
            return IRuleOutcome.Allowed("UnknownMethod", String.Join(", ", allowedReasons));
        }

        return IRuleOutcome.Ignored();
    }

    private void LogRules(IEnumerable<IRuleOutcome> outcomes)
    {
        foreach (var outcome in outcomes)
            LogSingleRule(outcome);
    }

    private void LogSingleRule(IRuleOutcome outcome)
    {
        if(outcome.IsBlocked())
            _logger.LogWarning(outcome.ToString());
        else if(outcome.IsAllowed())
            _logger.LogInformation(outcome.ToString());
        else
            _logger.LogDebug(outcome.ToString());
    }

        private void LogFinalRule<T>(
            CommsCheckItem item, 
            IRuleOutcome outcome) where T:IContactType
    {
        _logger.LogInformation(
            "Final Check on {T} for item {item} with outcome {outcome}", 
            typeof(T).Name, 
            item, 
            outcome.ToString());
    }


}