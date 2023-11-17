
namespace CommsCheck;

public readonly record struct GoneAway : IReasonForRemoval
{
    public string Code => "CGA";

    public bool NotSet => false;
    public bool HasCode => true;
    public bool IsEmpty => NotSet;
}
