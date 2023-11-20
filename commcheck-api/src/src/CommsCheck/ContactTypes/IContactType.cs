using System.Text.Json.Serialization;

namespace CommsCheck;

[JsonPolymorphic(UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization)]
[JsonDerivedType(typeof(App), typeDiscriminator: "App")]
[JsonDerivedType(typeof(Email), typeDiscriminator: "Email")]
[JsonDerivedType(typeof(Postal), typeDiscriminator: "Postal")]
[JsonDerivedType(typeof(Sms), typeDiscriminator: "Sms")]
public interface IContactType
{

}

