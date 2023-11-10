using MediatR;
using Microsoft.Extensions.ObjectPool;
using System.Diagnostics.Metrics;
using System.Threading.Channels;

public class CheckCommsCommandHandler(
    ObjectPool<HashWrapper> shaPool,
    //CommsCheckItemSha sha, 
    ChannelWriter<CommsCheckItemWithId> writer) : 
    IRequestHandler<CheckCommsCommand, CommsCheckQuestionResponseDto>
{

    private static readonly Meter MyMeter = new("NHS.CommChecker.CheckCommsCommandHandler", "1.0");
    private static readonly Counter<long> HandledCounter = MyMeter.CreateCounter<long>("CheckCommsCommandHandler_Handled_Count");

    public async Task<CommsCheckQuestionResponseDto> Handle(
        CheckCommsCommand request,
        CancellationToken cancellationToken)
    {
        var item = CommsCheckItem.FromDto(request.Dto);

        var wrapper = shaPool.Get();
        var pooledSha = await wrapper.GetSha(item, "Getting id in the Comms Handler.");
        shaPool.Return(wrapper);

        //var resultId = await sha.GetSha(item, "Getting id in the Comms Handler.");
        var itemWithId = new CommsCheckItemWithId(pooledSha, item);
        await writer.WriteAsync(itemWithId);

        HandledCounter.Add(1);
        return new CommsCheckQuestionResponseDto(pooledSha);
    }
}

public readonly record struct CommsCheckItemWithId (string Id, CommsCheckItem Item);
