public class SmsNoReasonfRemovalRule : ICommCheckRule<Sms>
{
    public IRuleOutcome Allowed(CommsCheckItem request)
     => 
      request.ReasonForRemoval switch
     {
        NoReasonForRemoval => IRuleOutcome.Allowed("Allowing sms due to no reason for removal is set."),
        _ => IRuleOutcome.Ignored()
     };     
}

