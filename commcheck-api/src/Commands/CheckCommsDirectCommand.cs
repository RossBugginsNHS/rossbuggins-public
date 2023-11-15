namespace CommsCheck;
using MediatR;

public class CheckCommsDirectCommand(CommsCheckQuestionRequestDto dto) : IRequest<CommsCheckAnswerResponseDto>
{
    public CommsCheckQuestionRequestDto Dto => dto;
}
