namespace CommsCheck;
using MediatR;

public class RulesLoadedEventHandlerAllow(
    RulesEngineAndHash _rulesEngine,
    ILogger<RulesLoadedEventHandlerAllow> _logger,
    RunRuleFunctions _ruleFunctions,
    IPublisher _publisher) : INotificationHandler<RulesLoadedEvent>
{
    public async Task Handle(RulesLoadedEvent request, CancellationToken cancellationToken)
    {
        var result = await _ruleFunctions.RunAllowed(
            request.Method,
             request.Method,
            _rulesEngine.RulesEngine,
            request.ToCheck.Item);

        _logger.LogInformation(
            "[{correlation}] Allowed rule check for {method} complete with outcome {outcome}",
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
