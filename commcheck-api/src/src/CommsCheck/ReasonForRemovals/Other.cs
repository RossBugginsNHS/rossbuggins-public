
namespace CommsCheck;

public readonly record struct Other : IReasonForRemoval
{
    public string Code => "ORR";

    public bool NotSet => false;
    public bool HasCode => true;
    public bool IsEmpty => NotSet;
}
