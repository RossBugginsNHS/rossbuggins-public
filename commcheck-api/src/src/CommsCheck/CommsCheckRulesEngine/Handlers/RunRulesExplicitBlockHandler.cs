namespace CommsCheck;

using MediatR;

public class RunRulesExplicitBlockHandler(
    RunRuleFunctions _ruleFunctions, 
    ILogger<RunRulesExplicitBlockHandler> _logger,
    IPublisher _publisher) : INotificationHandler<RulesLoadedEvent>
{
    public async Task Handle(RulesLoadedEvent request, CancellationToken cancellationToken)
    {
        var result = await _ruleFunctions.RunExplictBlock(
            request.Method,
            request.RulesEngine,
            request.ToCheck.Item);

        _logger.LogInformation(
            "[{correlation}] Explicit Block rule check for {method} complete with outcome {outcome}",
            request.CommCheckCorrelationId,
            request.Method,
            result.OutcomeDescription);

        await _publisher.Publish(new RulesRunCompleteEvent(
            request.CommCheckCorrelationId,
            request.RuleRunId,
            result,
            request.Method,
            request.ToCheck), cancellationToken);
    }
}
