
public readonly record struct CommsCheckAnswer(
    string ResultId, 
    string RequestString, 
    params IRuleOutcome[] Outcomes)
{
}
