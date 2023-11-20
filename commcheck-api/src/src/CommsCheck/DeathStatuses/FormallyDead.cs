namespace CommsCheck;

public readonly record struct FormallyDead : IDeathStatus
{
    public int DeathCode => 2;
    public string DeathStatusDescription => "Formal";
}