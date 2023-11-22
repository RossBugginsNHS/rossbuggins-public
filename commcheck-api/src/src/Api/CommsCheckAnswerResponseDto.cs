namespace CommsCheck;

public readonly record struct CommsCheckAnswerResponseDto(
    string ResultId, 
    CommsCheckQuestionRequestDto Request,
    CommsCheckAnswerDto Response, 
    CommsCheckItem Item,
    string RulesFileHash,
    RuleOutcomesDto RuleOutcomes
)
{
    public static CommsCheckAnswerResponseDto FromCommsCheckAnswer(CommsCheckAnswer answer)
    {
        return new CommsCheckAnswerResponseDto(
            answer.ResultId,
            answer.Request.ToDto(), 
            CommsCheckAnswerDto.FromCommsCheckAnswer(answer),
            answer.CommsCheckItemWithId.Item,
            answer.RuleHash,
            new RuleOutcomesDto(answer.Summaries.ToArray())
            );  
    }
}

