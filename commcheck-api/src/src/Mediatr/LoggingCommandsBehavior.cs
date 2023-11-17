namespace CommsCheck;
using MediatR;

public class LoggingCommandsBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingCommandsBehavior<TRequest, TResponse>> _logger;

    public LoggingCommandsBehavior(ILogger<LoggingCommandsBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {name}", typeof(TRequest).Name);
        var response = await next();
        _logger.LogInformation("Handled {name}", typeof(TResponse).Name);
        return response;
    }
}
