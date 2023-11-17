
namespace CommsCheck;

public readonly record struct ServiceDependent : IReasonForRemoval
{
    public string Code => "SDN";

    public bool NotSet => false;
    public bool HasCode => true;
    public bool IsEmpty => NotSet;
}