using System.Diagnostics;
using System.Diagnostics.Metrics;
using MediatR;
using Microsoft.Extensions.ObjectPool;

public class CheckCommsDirectCommandHandler(
    ObjectPool<HashWrapper> shaPool,
     ICommCheck check) :
    IRequestHandler<CheckCommsDirectCommand, CommsCheckAnswerResponseDto>
{

    private static readonly Meter MyMeter = new("NHS.CommChecker.CheckCommsDirectCommandHandler", "1.0");
    private static readonly Counter<long> HandledCounter = MyMeter.CreateCounter<long>("CheckCommsDirectCommandHandler_Handled_Count");
    private static readonly System.Diagnostics.Metrics.Histogram<double> ProcessTime =
        MyMeter.CreateHistogram<double>("CheckCommsDirectCommandHandler_Duration_Seconds");

    private static readonly System.Diagnostics.Metrics.UpDownCounter<long> CurrentlyProcessing =
        MyMeter.CreateUpDownCounter<long>("CheckCommsDirectCommandHandler_Active_Count");

    public async Task<CommsCheckAnswerResponseDto> Handle(CheckCommsDirectCommand request, CancellationToken cancellationToken)
    {
        var sw = Stopwatch.StartNew();
        CurrentlyProcessing.Add(1);
        var item = CommsCheckItem.FromDto(request.Dto);
        var wrapper = shaPool.Get();
        var pooledSha = await wrapper.GetSha(item, "Getting id in the Comms Handler.");
        shaPool.Return(wrapper);
        var itemWithId = new CommsCheckItemWithId(pooledSha, item);
        var answer = await check.Check(itemWithId);
        var itemDto = CommsCheckAnswerResponseDto.FromCommsCheckAnswer(answer);
        sw.Stop();
        HandledCounter.Add(1);
        ProcessTime.Record(sw.Elapsed.TotalSeconds);
        CurrentlyProcessing.Add(-1);
        return itemDto;
    }
}