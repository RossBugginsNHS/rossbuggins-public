namespace CommsCheck;

using MediatR;

public class RuleOutcomeComputedEventHandlerLogger(ILogger<RuleOutcomeComputedEventHandlerLogger> _logger) 
: INotificationHandler<RuleOutcomeComputedEvent>
{
    public Task Handle(RuleOutcomeComputedEvent notification, CancellationToken cancellationToken)
    {
         _logger.LogInformation(
            "[{correlationId}] Completed rule run with result {result} on {method} {itemId} with reason {reason}",
            notification.CommCheckCorrelationId,
            notification.Outcome.OutcomeDescription,
            notification.Method,
            notification.ToCheck.Id,
            notification.Outcome.Reason);
        
        return Task.CompletedTask;
    }
}
