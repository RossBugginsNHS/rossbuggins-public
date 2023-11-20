namespace CommsCheck;

using System.Runtime.CompilerServices;
using System.Threading.Channels;

public class MostRecent100Cache(ILogger<MostRecent100Cache> _logger)
{
    private List<CommsCheckAnswerResponseDto> _data =
         new List<CommsCheckAnswerResponseDto>();

    private readonly Channel<CommsCheckAnswerResponseDto> _channel =
        Channel.CreateUnbounded<CommsCheckAnswerResponseDto>();

    private readonly List<Channel<CommsCheckAnswerResponseDto>> _streamchannels =
        new List<Channel<CommsCheckAnswerResponseDto>>();

    private Channel<CommsCheckAnswerResponseDto> SetupStream()
    {
        _logger.LogInformation("Streaming started.");
        var streamChannel = Channel.CreateUnbounded<CommsCheckAnswerResponseDto>();
        _streamchannels.Add(streamChannel);
        return streamChannel;
    }

    private void FinishStream(Channel<CommsCheckAnswerResponseDto> streamChannel)
    {
        _logger.LogInformation("Streaming ended.");
        _streamchannels.Remove(streamChannel);
    }

    public async IAsyncEnumerable<CommsCheckAnswerResponseDto> GetStream(
        [EnumeratorCancellation] CancellationToken ct)
    {
        var streamChannel = SetupStream();
        CancellationTokenSource cts = new CancellationTokenSource(); 
        ct.Register(()=> 
        {
             FinishStream(streamChannel);
             cts.Cancel();
        });

        await foreach (var item in streamChannel.Reader.ReadAllAsync(cts.Token))
        {
            if (cts.IsCancellationRequested)
                break;

            yield return item;
        }
    }

    public ChannelReader<CommsCheckAnswerResponseDto> GetReader() => _channel.Reader;

    public void UpdateData(IEnumerable<CommsCheckAnswerResponseDto> data)
    {
        _data = data.ToList();
    }

    public async Task Add(ItemCacheUpdatedEvent notification)
    {
        if (notification.Answer.Outcomes.Length >= 4)
        {
            var dto = CommsCheckAnswerResponseDto.FromCommsCheckAnswer(notification.Answer);
            await _channel.Writer.WriteAsync(dto);

            foreach (var streamChannel in _streamchannels)
            {
                await streamChannel.Writer.WriteAsync(dto);
            }
        }
    }

    public IEnumerable<CommsCheckAnswerResponseDto> Get()
    {
        var x = _data;
        return x;
    }
}