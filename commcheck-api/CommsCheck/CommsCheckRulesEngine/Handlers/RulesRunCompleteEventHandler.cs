namespace CommsCheck;
using MediatR;

public class RulesRunCompleteEventHandler(RulesCombinerService _combiner) 
: INotificationHandler<RulesRunCompleteEvent>
{
    public async Task Handle(RulesRunCompleteEvent notification, CancellationToken cancellationToken)
    {
        await _combiner.Combine(notification);
    }
}
