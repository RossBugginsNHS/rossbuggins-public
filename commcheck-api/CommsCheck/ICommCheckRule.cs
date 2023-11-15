namespace CommsCheck;
public interface ICommCheckRule
{
    public IRuleOutcome Block(string method, CommsCheckItem request) => IRuleOutcome.Ignored();
}


public interface ICommCheckRule<T> : ICommCheckRule where T : IContactType
{
        public IRuleOutcome Allowed(string method, CommsCheckItem request) => IRuleOutcome.Ignored();
}
