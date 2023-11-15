namespace CommsCheck;

using MediatR;

public class RunRulesExplicitBlockHandler(IPublisher _publisher) : INotificationHandler<RulesLoadedEvent>
{
    public async Task Handle(RulesLoadedEvent request, CancellationToken cancellationToken)
    {
        var result = await RunRuleFunctions.RunExplictBlock(
            request.Method,
            request.RulesEngine,
            request.ToCheck.Item);

        await _publisher.Publish(new RulesRunCompleteEvent(
            request.RuleRunId,
            result,
            request.Method,
            request.ToCheck));
    }
}
