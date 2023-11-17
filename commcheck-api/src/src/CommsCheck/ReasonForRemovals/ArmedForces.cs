
namespace CommsCheck;

public readonly record struct ArmedForces : IReasonForRemoval
{
    public string Code => "AFN";

    public bool NotSet => false;
    public bool HasCode => true;
    public bool IsEmpty => NotSet;
}

