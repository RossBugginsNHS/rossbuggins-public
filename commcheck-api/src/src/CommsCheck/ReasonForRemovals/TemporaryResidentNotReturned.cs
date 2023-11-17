
namespace CommsCheck;

public readonly record struct TemporaryResidentNotReturned : IReasonForRemoval
{
    public string Code => "TRA";

    public bool NotSet => false;
    public bool HasCode => true;
    public bool IsEmpty => NotSet;
}