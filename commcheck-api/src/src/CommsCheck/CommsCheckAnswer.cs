
namespace CommsCheck;
public readonly record struct CommsCheckAnswer(
    string ResultId, 
    string RequestString, 
    DateTime StartedAt,
    DateTime  UpdatedAt,
    int UpdatedCount,
    params IRuleOutcome[] Outcomes)
{
}
