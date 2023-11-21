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
    public async Task Run(Guid commCheckCorrelationId, CommsCheckItemWithId toCheck)
    {
        var currentMethod = GetMethod();
        var ruleRunId = NewRuleRunId();
        
        Starting(commCheckCorrelationId, toCheck, currentMethod, ruleRunId);
        await _publisher.Publish(
            new RulesLoadedEvent(commCheckCorrelationId, ruleRunId, currentMethod, toCheck));
    }

    private void Starting(
        Guid commCheckCorrelationId,
        CommsCheckItemWithId toCheck,
        string currentMethod,
        Guid ruleRunId
        )
    {
        _logger.LogInformation(
           "[{CorrelationId}] Running Rules for {checkId} for {method} with localId of {localId}",
            commCheckCorrelationId,
            toCheck.Id,
            currentMethod,
            ruleRunId);
    }

    private static string GetMethod() => typeof(T).Name;

    private static Guid NewRuleRunId() => Guid.NewGuid();
}
