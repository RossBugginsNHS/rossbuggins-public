namespace CommsCheck;

using MediatR;

public class RuleResultsCombinedEventHandler(IPublisher _publisher) : INotificationHandler<RuleResultsCombinedEvent>
{
    public async Task Handle(RuleResultsCombinedEvent notification, CancellationToken cancellationToken)
    {
        var blocked = notification.Outcomes.Where(x => x is RuleBlocked).ToList();
        var allowed = notification.Outcomes.Where(x => x is RuleAllowed).ToList();

        var t = (blocked.Count > 0, allowed.Count > 0) switch
        {
            (true, _) => _publisher.Publish(new RuleOutcomeComputedEvent(
                notification.CommCheckCorrelationId,
                notification.RuleRunId,
                notification.RuleHash,
                notification.Method,
                notification.ToCheck,
                blocked[0],
                notification.Summaries), cancellationToken),

            (false, true) => _publisher.Publish(new RuleOutcomeComputedEvent(
                notification.CommCheckCorrelationId,
                notification.RuleRunId,
                notification.RuleHash,
                notification.Method,
                notification.ToCheck,
                allowed[0],
                notification.Summaries), cancellationToken),

            _ => _publisher.Publish(new RuleOutcomeComputedEvent(
                notification.CommCheckCorrelationId,
                notification.RuleRunId,
                notification.RuleHash,
                notification.Method,
                notification.ToCheck,
                Blocked(notification.Method),
                notification.Summaries), cancellationToken)
        };
        await t;
    }

    private IRuleOutcome Blocked(string method) =>
        IRuleOutcome.Blocked(
            RunRuleFunctions.RuleSetExplictBlock, 
            method, 
            RunRuleFunctions.DefaultBlock);
}
