namespace CommsCheck;

public readonly record struct CommsCheckAnswerResponseDto(
    string ResultId, 
    CommsCheckQuestionRequestDto Request, 
    CommsCheckItem Item,
    string RulesFileHash,
    CommsCheckAnswerDto Response
)
{
    public static CommsCheckAnswerResponseDto FromCommsCheckAnswer(CommsCheckAnswer answer)
    {
        return new CommsCheckAnswerResponseDto(
            answer.ResultId,
            answer.Request.ToDto(), 
            answer.CommsCheckItemWithId.Item,
            answer.RuleHash,
            CommsCheckAnswerDto.FromCommsCheckAnswer(answer));  
    }


}
