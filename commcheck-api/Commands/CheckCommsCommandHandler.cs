using MediatR;
using System.Threading.Channels;

public class CheckCommsCommandHandler(
    CommsCheckItemSha sha, 
    ChannelWriter<CommsCheckItemWithId> writer) : 
    IRequestHandler<CheckCommsCommand, CommsCheckQuestionResponseDto>
{
    public async Task<CommsCheckQuestionResponseDto> Handle(
        CheckCommsCommand request,
        CancellationToken cancellationToken)
    {
        var item = CommsCheckItem.FromDto(request.Dto);
        var resultId = await sha.GetSha(item, "Getting id in the Comms Handler.");
        var itemWithId = new CommsCheckItemWithId(resultId, item);
        await writer.WriteAsync(itemWithId);
        return new CommsCheckQuestionResponseDto(resultId);
    }
}

public readonly record struct CommsCheckItemWithId (string Id, CommsCheckItem Item);
