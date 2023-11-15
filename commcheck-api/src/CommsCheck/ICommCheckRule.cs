namespace CommsCheck;
public interface ICommCheckRule
{
    public IRuleOutcome Block(string method, CommsCheckItem request) => IRuleOutcome.Ignored();
}


public interface ICommCheckRule<out T> : ICommCheckRule where T : IContactType
{
    public T Instance{get;}
    public IRuleOutcome Allowed(string method, CommsCheckItem request) => IRuleOutcome.Ignored();
}
