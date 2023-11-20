namespace CommsCheck;

public readonly record struct InformallyDead : IDeathStatus
{
    public int DeathCode => 1;
    public string DeathStatusDescription => "Informal";
}
