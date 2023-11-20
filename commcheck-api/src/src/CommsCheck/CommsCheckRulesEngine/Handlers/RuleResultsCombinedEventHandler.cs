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
                blocked[0]), cancellationToken),

            (false, true) => _publisher.Publish(new RuleOutcomeComputedEvent(
                notification.CommCheckCorrelationId,
                notification.RuleRunId,
                notification.RuleHash,
                notification.Method,
                notification.ToCheck,
                allowed[0]), cancellationToken),

            _ => _publisher.Publish(new RuleOutcomeComputedEvent(
                notification.CommCheckCorrelationId,
                notification.RuleRunId,
                notification.RuleHash,
                notification.Method,
                notification.ToCheck,
                IRuleOutcome.Blocked(notification.Method, "Default Block"))
                , cancellationToken)
        };

        await t;
    }
}
