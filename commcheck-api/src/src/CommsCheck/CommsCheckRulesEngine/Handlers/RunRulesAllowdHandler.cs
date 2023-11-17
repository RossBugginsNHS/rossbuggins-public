namespace CommsCheck;
using MediatR;

public class RunRulesAllowdHandler(
    ILogger<RunRulesAllowdHandler> _logger,
    RunRuleFunctions _ruleFunctions, 
    IPublisher _publisher) : INotificationHandler<RulesLoadedEvent>
{
    public async Task Handle(RulesLoadedEvent request, CancellationToken cancellationToken)
    {
        var result = await _ruleFunctions.RunAllowed(
            request.Method,
            request.RulesEngine,
            request.ToCheck.Item);

        _logger.LogInformation(
            "[{correlation}] Allowed rule check for {method} complete with outcome {outcome}",
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
