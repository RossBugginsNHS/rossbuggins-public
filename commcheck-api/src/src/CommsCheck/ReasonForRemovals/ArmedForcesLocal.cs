
namespace CommsCheck;

public readonly record struct ArmedForcesLocal : IReasonForRemoval
{
    public string Code => "AFL";

    public bool NotSet => false;
    public bool HasCode => true;
    public bool IsEmpty => NotSet;
}
