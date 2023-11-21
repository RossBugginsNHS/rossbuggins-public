namespace CommsCheck;

using MediatR;

public class RulesLoadedEventHandlerExplicitBlock(
    RulesEngineAndHash _rulesEngine,
    RunRuleFunctions _ruleFunctions, 
    ILogger<RulesLoadedEventHandlerExplicitBlock> _logger,
    IPublisher _publisher) : INotificationHandler<RulesLoadedEvent>
{
    public async Task Handle(RulesLoadedEvent request, CancellationToken cancellationToken)
    {
        var resultAndSummary = await _ruleFunctions.RunExplictBlock(
            request.Method,
            _rulesEngine.RulesEngine,
            request.ToCheck.Item);

         var result = resultAndSummary.Outcome;

        _logger.LogInformation(
            "[{correlation}] Explicit Block rule check for {method} complete with outcome {outcome}",
            request.CommCheckCorrelationId,
            request.Method,
            result.OutcomeDescription);

        await _publisher.Publish(new RulesRunCompletedEvent(
            request.CommCheckCorrelationId,
            request.RuleRunId,
            _rulesEngine.RulesHash,
            result,
            request.Method,
            request.ToCheck,
            resultAndSummary.Summaries.ToList()), cancellationToken);
    }
}
