
namespace CommsCheck;

public readonly record struct PractiseDissolution : IReasonForRemoval
{
    public string Code => "DIS";

    public bool NotSet => false;
    public bool HasCode => true;
    public bool IsEmpty => NotSet;
}
