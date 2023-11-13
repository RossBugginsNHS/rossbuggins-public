[Obsolete("Use CommsCheckRulesEngine instead.", false)]
public class Over115ExplicitRule : ICommCheckRule
{
    public IRuleOutcome Block(string method, CommsCheckItem request)
     => 
      (request.YearsOld) switch
     {
         < 0     => IRuleOutcome.Blocked(method, "Less than 0 years old"),
         >= 115  => IRuleOutcome.Blocked (method, "Over 115"),
         _       => IRuleOutcome.Ignored()
     };
}

