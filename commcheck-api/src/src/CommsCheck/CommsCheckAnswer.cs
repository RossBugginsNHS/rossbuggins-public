
namespace CommsCheck;
public readonly record struct CommsCheckAnswer(
    string ResultId, 
    string RuleHash,
    CommsCheckItemWithId CommsCheckItemWithId, 
    CommsCheckQuestionRequestDtoCopy Request, 
    DateOnly RelativeDate,
    DateTime StartedAt,
    DateTime  UpdatedAt,
    int UpdatedCount,
    IEnumerable<RuleResultSummary> Summaries,
    params IRuleOutcome[] Outcomes)
{
}
