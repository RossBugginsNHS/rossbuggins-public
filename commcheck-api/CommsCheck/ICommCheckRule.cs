public interface ICommCheckRule
{
    public IRuleOutcome Block(CommsCheckItem request) => IRuleOutcome.Ignored();
}


public interface ICommCheckRule<T> : ICommCheckRule where T : IContactType
{
        public IRuleOutcome Allowed(CommsCheckItem request) => IRuleOutcome.Ignored();
}
