namespace CommsCheck;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;

public class CommsCheckRulesEngineRuleRunEvents<T>(
    ILogger<CommsCheckRulesEngineRuleRunEvents<T>> _logger,
    IPublisher _publisher) :
    ICommsCheckRulesEngineRuleRun<T> where T : class, IContactType, new()
{
    public T Value => new T();
    public async Task Run(RulesEngine.RulesEngine rulesEngine, CommsCheckItemWithId toCheck)
    {
       
        var currentMethod = GetMethod();
        var ruleRunId = Guid.NewGuid();
         _logger.LogInformation("Running Rules for {checkId} for {method} with localId of {localId}", toCheck.Id, currentMethod, ruleRunId);
        await _publisher.Publish(
            new RulesLoadedEvent(ruleRunId, rulesEngine, currentMethod, toCheck));
    }

    private static string GetMethod() => typeof(T).Name;
}
