using MediatR;

public class RunRuleEvent(
    ICommsCheckRulesEngineRuleRun<IContactType> rule,
    RulesEngine.RulesEngine rules,
    CommsCheckItemWithId toCheck)
    : INotification
{
    public ICommsCheckRulesEngineRuleRun<IContactType> Rule => rule;
    public RulesEngine.RulesEngine Rules => rules;
    public CommsCheckItemWithId ToCheck => toCheck;
}
