namespace CommsCheck;

using MediatR;

public class RulesLoadedEventHandlerExplicitBlockAll(
    RulesEngineAndHash _rulesEngine,
    ILogger<RulesLoadedEventHandlerExplicitBlockAll> _logger,
    RunRuleFunctions _ruleFunctions, 
    IPublisher _publisher) : INotificationHandler<RulesLoadedEvent>
{
    public async Task Handle(RulesLoadedEvent request, CancellationToken cancellationToken)
    {
        var result = await _ruleFunctions.RunExplictBlockAll(
            request.Method,
            _rulesEngine.RulesEngine,
            request.ToCheck.Item);

        _logger.LogInformation(
            "[{correlation}] Explicit Block All rule check for {method} complete with outcome {outcome}",
            request.CommCheckCorrelationId,
            request.Method,
            result.OutcomeDescription);

        await _publisher.Publish(new RulesRunCompletedEvent(
            request.CommCheckCorrelationId,
            request.RuleRunId,
            _rulesEngine.RulesHash,
            result,
            request.Method,
            request.ToCheck), cancellationToken);
    }
}
