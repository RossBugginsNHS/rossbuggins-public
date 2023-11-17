
namespace CommsCheck;

public readonly record struct ServiceDependentLocal : IReasonForRemoval
{
    public string Code => "SDL";

    public bool NotSet => false;
    public bool HasCode => true;
    public bool IsEmpty => NotSet;
}
