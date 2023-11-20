namespace CommsCheck;

using MediatR;

public class RunRuleStartedEventHandler : INotificationHandler<RunRuleStartedEvent>
{
    public async Task Handle(RunRuleStartedEvent notification, CancellationToken cancellationToken)
    {
        await notification.Rule.Run(notification.CommCheckCorrelationId, notification.ToCheck);
    }
}