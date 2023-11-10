[Obsolete("Use CommsCheckRulesEngine instead.", false)]
public class SmsNoReasonfRemovalRule : ICommCheckRule<Sms>
{
    public IRuleOutcome Allowed(string method, CommsCheckItem request)
     => 
      request.ReasonForRemoval switch
     {
        NoReasonForRemoval => IRuleOutcome.Allowed(method, "Allowing sms due to no reason for removal is set."),
        _ => IRuleOutcome.Ignored()
     };     
}

