using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Rewrite;

[JsonPolymorphic(
    UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToNearestAncestor)]
[JsonDerivedType(typeof(RuleAllowed), typeDiscriminator: "RuleAllowed")]
[JsonDerivedType(typeof(RuleBlocked), typeDiscriminator: "RuleBlocked")]
[JsonDerivedType(typeof(RuleIgnored), typeDiscriminator: "RuleIgnored")]
public interface IRuleOutcome : IEquatable<IRuleOutcome>
{
    string Reason{get;}
    public static IRuleOutcome Allowed(string reason) => new RuleAllowed(reason);
    public static IRuleOutcome Blocked(string reason) => new RuleBlocked(reason);
    public static IRuleOutcome Ignored()=> new RuleIgnored();

    public bool IsAllowed()
    {
        return this is RuleAllowed;
    }

    public bool IsBlocked()
    {
        return this is RuleBlocked;
    }
}


