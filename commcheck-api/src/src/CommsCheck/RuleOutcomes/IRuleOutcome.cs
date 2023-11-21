namespace CommsCheck;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Rewrite;

[JsonPolymorphic(UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization)]
[JsonDerivedType(typeof(RuleAllowed), typeDiscriminator: "RuleAllowed")]
[JsonDerivedType(typeof(RuleBlocked), typeDiscriminator: "RuleBlocked")]
[JsonDerivedType(typeof(RuleIgnored), typeDiscriminator: "RuleIgnored")]
public interface IRuleOutcome : IEquatable<IRuleOutcome>
{
    string RuleSet{get;}
    string Method{get;}
    string Reason{get;}
    public static IRuleOutcome Allowed(string ruleSet, string method, string reason) => new RuleAllowed(ruleSet, method, reason);
    public static IRuleOutcome Blocked(string ruleSet, string method, string reason) => new RuleBlocked(ruleSet, method, reason);
    public static IRuleOutcome Ignored()=> new RuleIgnored();

    bool IsAllowed{get;}
    bool IsBlocked{get;}

    string OutcomeDescription{get;}

}


