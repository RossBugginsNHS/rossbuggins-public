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
    public async Task Run(Guid commCheckCorrelationId, RulesEngine.RulesEngine rulesEngine, CommsCheckItemWithId toCheck)
    {
        var currentMethod = GetMethod();
         _logger.LogInformation("Running Rules for {checkId} for {method} with localId of {localId}", toCheck.Id, currentMethod, commCheckCorrelationId);
        await _publisher.Publish(
            new RulesLoadedEvent(commCheckCorrelationId, rulesEngine, currentMethod, toCheck));
    }

    private static string GetMethod() => typeof(T).Name;
}
