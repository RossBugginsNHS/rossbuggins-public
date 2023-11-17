namespace CommsCheck;
using MediatR;

public class CheckCommsCommand(CommsCheckQuestionRequestDto dto) : IRequest<CommsCheckQuestionResponseDto>
{
    public CommsCheckQuestionRequestDto Dto => dto;
}
