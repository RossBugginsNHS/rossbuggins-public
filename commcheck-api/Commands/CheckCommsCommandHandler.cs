using MediatR;
using System.Threading.Channels;

public class CheckCommsCommandHandler(
    CommsCheckItemSha sha, 
    ChannelWriter<CommsCheckItem> writer) : 
    IRequestHandler<CheckCommsCommand, CommsCheckQuestionResponseDto>
{
    public async Task<CommsCheckQuestionResponseDto> Handle(
        CheckCommsCommand request,
        CancellationToken cancellationToken)
    {
        var item = CommsCheckItem.FromDto(request.Dto);
        await writer.WriteAsync(item);
        var resultId = sha.GetSha(item);
        return new CommsCheckQuestionResponseDto(resultId);
    }
}
