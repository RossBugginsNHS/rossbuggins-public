namespace CommsCheck;

public readonly record struct NoDeathStatus : IDeathStatus
{
    public int DeathCode => 0;
    public string DeathStatusDescription => "None";
}
