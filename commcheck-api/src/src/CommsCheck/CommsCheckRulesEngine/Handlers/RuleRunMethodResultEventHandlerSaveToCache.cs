namespace CommsCheck;

using MediatR;

public class RuleRunMethodResultEventHandlerSaveToCache(RuleRunMethodResultCacheService _cache) 
: INotificationHandler<RuleRunMethodResultEvent>
{
    public async Task Handle(RuleRunMethodResultEvent notification, CancellationToken cancellationToken)
    {
        await _cache.NewOrUpdateCacheThreadSafe(notification, cancellationToken);
    }
}

public class RuleRunMethodResultEventHandlerLog(ILogger<RuleRunMethodResultEventHandlerLog> _logger) 
: INotificationHandler<RuleRunMethodResultEvent>
{
    public Task Handle(RuleRunMethodResultEvent notification, CancellationToken cancellationToken)
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
