namespace CommsCheck;

public readonly record struct MovedAway : IReasonForRemoval
{
    public string Code => "CGA";
}
