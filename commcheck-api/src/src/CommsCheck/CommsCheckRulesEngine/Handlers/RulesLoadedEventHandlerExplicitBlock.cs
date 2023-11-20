namespace CommsCheck;

using MediatR;

public class RulesLoadedEventHandlerExplicitBlock(
    RulesEngine.RulesEngine _rulesEngine,
    RunRuleFunctions _ruleFunctions, 
    ILogger<RulesLoadedEventHandlerExplicitBlock> _logger,
    IPublisher _publisher) : INotificationHandler<RulesLoadedEvent>
{
    public async Task Handle(RulesLoadedEvent request, CancellationToken cancellationToken)
    {
        var result = await _ruleFunctions.RunExplictBlock(
            request.Method,
            _rulesEngine,
            request.ToCheck.Item);

        _logger.LogInformation(
            "[{correlation}] Explicit Block rule check for {method} complete with outcome {outcome}",
            request.CommCheckCorrelationId,
            request.Method,
            result.OutcomeDescription);

        await _publisher.Publish(new RulesRunCompletedEvent(
            request.CommCheckCorrelationId,
            request.RuleRunId,
            result,
            request.Method,
            request.ToCheck), cancellationToken);
    }
}
