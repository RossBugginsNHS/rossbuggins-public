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
        await foreach (var item in _reader.ReadAllAsync(stoppingToken))
        {
           await _publisher.Publish(new MaybeItemToCheckEvent(item), stoppingToken);
        }
    }
}
