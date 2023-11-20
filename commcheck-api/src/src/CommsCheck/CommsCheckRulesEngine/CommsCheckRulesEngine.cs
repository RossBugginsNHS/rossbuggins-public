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
        _logger.LogInformation("[{commCheckCorrelationId}] Parallel invoking of rules started", commCheckCorrelationId);
        await Parallel.ForEachAsync(_rules, cancellationToken, async (rule, ct) =>
        {
            _logger.LogInformation("[{commCheckCorrelationId}] Parallel invoking of rule started", commCheckCorrelationId);
            await _publisher.Publish(new RunRuleStartedEvent(commCheckCorrelationId, rule, toCheck), ct);
        });
        _logger.LogInformation("[{commCheckCorrelationId}] Parallel invoking of rules completed", commCheckCorrelationId);
    }
}
