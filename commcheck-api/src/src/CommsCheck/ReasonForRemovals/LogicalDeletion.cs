
namespace CommsCheck;

public readonly record struct LogicalDeletion : IReasonForRemoval
{
    public string Code => "LDN";

    public bool NotSet => false;
    public bool HasCode => true;
    public bool IsEmpty => NotSet;
}
