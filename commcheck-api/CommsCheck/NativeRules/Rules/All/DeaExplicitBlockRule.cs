[Obsolete("Use CommsCheckRulesEngine instead.", false)]
public class DeaExplicitBlockRule : ICommCheckRule
{
    public IRuleOutcome Block(string method, CommsCheckItem request)
     => 
      request.ReasonForRemoval switch
     {
        Death => IRuleOutcome.Blocked(method, "Death code is detected"),
        _ => IRuleOutcome.Ignored()
     };
}

