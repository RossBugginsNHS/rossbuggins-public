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
            (true, _) => _publisher.Publish(new RuleRunMethodResultEvent(
                notification.RuleRubId,
                notification.Method,
                notification.ToCheck,
                blocked.First())),

            (false, true) => _publisher.Publish(new RuleRunMethodResultEvent(
                notification.RuleRubId,
                notification.Method,
                notification.ToCheck,
                allowed.First())),

            _ => _publisher.Publish(new RuleRunMethodResultEvent(
                notification.RuleRubId,
                notification.Method,
                notification.ToCheck,
                IRuleOutcome.Blocked(notification.Method, "Default Block")))
        };

        await t;
    }
}
