namespace CommsCheck;

using System.Collections.Concurrent;
using System.Text.Json;
using FastExpressionCompiler;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

public class CommsCheckRulesEngine : ICommCheck
{
    public const string CommsCheckRuleEngineCacheKey = "CommsCheckRulesEngineFileData";

    private readonly ILogger<CommsCheckRulesEngine> _logger;
    private readonly IEnumerable<ICommsCheckRulesEngineRuleRun<IContactType>> _rules;
    private readonly IPublisher _publisher;

    public CommsCheckRulesEngine(
         ILogger<CommsCheckRulesEngine> logger,
         IEnumerable<ICommsCheckRulesEngineRuleRun<IContactType>> rules,
         IPublisher publisher)
    {
        _logger = logger;
        _rules = rules;
        _publisher = publisher;
    }

    public async Task Check(
        Guid commCheckCorrelationId,
        CommsCheckItemWithId toCheck,
        CancellationToken cancellationToken = default)
    {
        Starting(commCheckCorrelationId);
        await Parallel.ForEachAsync(_rules, cancellationToken, async (rule, ct) =>
        {
            await InvokeRule(commCheckCorrelationId, toCheck, rule, ct);
        });

        Completed(commCheckCorrelationId);
    }

    private async Task InvokeRule(
         Guid commCheckCorrelationId,
        CommsCheckItemWithId toCheck,
        ICommsCheckRulesEngineRuleRun<IContactType> rule,
        CancellationToken cancellationToken = default)
    {
        Invoking(commCheckCorrelationId);
        await _publisher.Publish(
            new RunRuleStartedEvent(commCheckCorrelationId, rule, toCheck), cancellationToken);
    }

    private void Starting(Guid commCheckCorrelationId)
    {
        _logger.LogInformation("[{commCheckCorrelationId}] Parallel invoking of rules started", commCheckCorrelationId);
    }

    private void Invoking(Guid commCheckCorrelationId)
    {
        _logger.LogInformation("[{commCheckCorrelationId}] Parallel invoking of rule started", commCheckCorrelationId);
    }

    private void Completed(Guid commCheckCorrelationId)
    {
        _logger.LogInformation("[{commCheckCorrelationId}] Parallel invoking of rules completed", commCheckCorrelationId);
    }
}
