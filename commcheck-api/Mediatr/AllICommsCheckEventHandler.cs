using MediatR;

public class NotificationLogger<T> : INotificationHandler<T>
    where T : ICommsCheckEvent
{
    private readonly ILogger<NotificationLogger<T>> _logger;
    public NotificationLogger(ILogger<NotificationLogger<T>> logger)
    {
        _logger = logger;
    }
    public Task Handle(T notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"[{notification.GetType().Name}]: {notification} ");
        return Task.CompletedTask;
    }
}
