
namespace CommsCheck;

public readonly record struct X : IReasonForRemoval
{
    public string Code => "X";

    public bool NotSet => false;
    public bool HasCode => true;
    public bool IsEmpty => NotSet;
}

