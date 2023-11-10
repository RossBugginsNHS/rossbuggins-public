public class Over115ExplicitRule : ICommCheckRule
{
    public IRuleOutcome Block(CommsCheckItem request)
     => 
      (request.YearsOld) switch
     {
         < 0     => IRuleOutcome.Blocked("Less than 0 years old"),
         >= 115  => IRuleOutcome.Blocked ("Over 115"),
         _       => IRuleOutcome.Ignored()
     };
}

