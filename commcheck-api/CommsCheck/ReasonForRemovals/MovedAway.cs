namespace CommsCheck;

public readonly record struct MovedAway : IReasonForRemoval
{
    public string Code => "CGA";
    public bool NotSet => false;
    public bool HasCode => true;
    public bool IsEmpty() => NotSet;
}
