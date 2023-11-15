namespace CommsCheck;

using MediatR;

public class RunRuleEventHandler : INotificationHandler<RunRuleEvent>
{
    public async Task Handle(RunRuleEvent notification, CancellationToken cancellationToken)
    {
        await notification.Rule.Run(notification.Rules, notification.ToCheck);
    }
}