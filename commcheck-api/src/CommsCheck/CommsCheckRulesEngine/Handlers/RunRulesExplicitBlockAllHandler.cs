namespace CommsCheck;

using MediatR;

public class RunRulesExplicitBlockAllHandler(IPublisher _publisher) : INotificationHandler<RulesLoadedEvent>
{
    public async Task Handle(RulesLoadedEvent request, CancellationToken cancellationToken)
    {
        var result = await RunRuleFunctions.RunExplictBlockAll(
            request.Method,
            request.RulesEngine,
            request.ToCheck.Item);

        await _publisher.Publish(new RulesRunCompleteEvent(
            request.CommCheckCorrelationId,
            request.RuleRunId,
            result,
            request.Method,
            request.ToCheck), cancellationToken);
    }
}
