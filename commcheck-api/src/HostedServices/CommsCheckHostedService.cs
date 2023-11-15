namespace CommsCheck;
using System.Text;
using System.Threading.Channels;
using MediatR;

public class CommsCheckHostedService(

    ChannelReader<CommsCheckItemWithId> _reader,
    IPublisher _publisher
    ) : BackgroundService
{

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var correlationId = NewCorrelationId();
        await foreach (var item in _reader.ReadAllAsync(stoppingToken))
        {
           await _publisher.Publish(new MaybeItemToCheckEvent(correlationId, item), stoppingToken);
        }
    }

    private static Guid NewCorrelationId() =>Guid.NewGuid();
}
