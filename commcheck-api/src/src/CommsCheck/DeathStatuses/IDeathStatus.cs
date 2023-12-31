using System.Text.Json.Serialization;

namespace CommsCheck;

[JsonPolymorphic(UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization)]
[JsonDerivedType(typeof(FormallyDead), typeDiscriminator: "FormallyDead")]
[JsonDerivedType(typeof(InformallyDead), typeDiscriminator: "InformallyDead")]
[JsonDerivedType(typeof(NoDeathStatus), typeDiscriminator: "NoDeathStatus")]
public interface IDeathStatus
{
    int DeathCode { get; }
    string DeathStatusDescription { get; }

    public static IDeathStatus FromEnum(DeathStatus? status) =>
    
        status switch
        {
            null => IDeathStatus.NoDeathStatus,
            DeathStatus.None => IDeathStatus.NoDeathStatus,
            DeathStatus.INFORMAL => IDeathStatus.InformallyDead,
            DeathStatus.FORMAL => IDeathStatus.FormallyDead,
            _ => throw new ArgumentOutOfRangeException(nameof(status), "Death status enum value not found")
        };

    public static NoDeathStatus NoDeathStatus => new NoDeathStatus();
    public static InformallyDead InformallyDead => new InformallyDead();
    public static FormallyDead FormallyDead => new FormallyDead();
}
