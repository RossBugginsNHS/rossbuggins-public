namespace CommsCheck;
using MediatR;

public class AllICommsCheckEventHandler<T> : INotificationHandler<T>
    where T : ICommsCheckEvent
{
    private readonly ILogger<AllICommsCheckEventHandler<T>> _logger;
    public AllICommsCheckEventHandler(ILogger<AllICommsCheckEventHandler<T>> logger)
    {
        _logger = logger;
    }
    public Task Handle(T notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[{name}]: {notification}", notification.GetType().Name, notification);
        return Task.CompletedTask;
    }
}
