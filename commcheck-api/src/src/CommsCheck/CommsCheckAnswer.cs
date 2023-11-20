
namespace CommsCheck;
public readonly record struct CommsCheckAnswer(
    string ResultId, 
    CommsCheckQuestionRequestDtoCopy Request, 
    DateOnly RelativeDate,
    DateTime StartedAt,
    DateTime  UpdatedAt,
    int UpdatedCount,
    params IRuleOutcome[] Outcomes)
{
}
