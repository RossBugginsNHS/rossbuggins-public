
public readonly record struct CommsCheckAnswer(
    string ResultId, 
    string RequestString, 
    IRuleOutcome App, 
    IRuleOutcome Email, 
    IRuleOutcome SMS, 
    IRuleOutcome Postal)
{
}
