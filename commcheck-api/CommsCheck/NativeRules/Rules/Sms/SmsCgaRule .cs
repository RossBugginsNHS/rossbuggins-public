[Obsolete("Use CommsCheckRulesEngine instead.", false)]
public class SmsCgaRule : ICommCheckRule<Sms>
{
    public IRuleOutcome Allowed(string method, CommsCheckItem request)
     => 
      (request.ReasonForRemoval, request.DaySinceSmsUpdate) switch
     {
        (MovedAway, <= 365) => IRuleOutcome.Allowed(method, "Allowing sms when moved away is set and sms updated in last 365 days."),
        _ => IRuleOutcome.Ignored()
     };     
}

