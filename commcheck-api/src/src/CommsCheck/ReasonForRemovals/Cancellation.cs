
namespace CommsCheck;

public readonly record struct Cancellation : IReasonForRemoval
{
    public string Code => "CAN";

    public bool NotSet => false;
    public bool HasCode => true;
    public bool IsEmpty => NotSet;
}
