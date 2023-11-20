namespace CommsCheck;
using MediatR;

public class RulesRunCompletedEventHandler(RulesCombinerService _combiner) 
: INotificationHandler<RulesRunCompletedEvent>
{
    private static int finishedCombiningWhenOutcomesCountIs = 3;
    public async Task Handle(RulesRunCompletedEvent notification, CancellationToken cancellationToken)
    {
        await _combiner.Combine(notification, finishedCombiningWhenOutcomesCountIs);
    }
}
