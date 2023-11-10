public class DeaExplicitBlockRule : ICommCheckRule
{
    public IRuleOutcome Block(CommsCheckItem request)
     => 
      request.ReasonForRemoval switch
     {
        Death => IRuleOutcome.Blocked("Death code is detected"),
        _ => IRuleOutcome.Ignored()
     };
}

