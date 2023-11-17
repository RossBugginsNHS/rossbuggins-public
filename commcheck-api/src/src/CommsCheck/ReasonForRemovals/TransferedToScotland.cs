
namespace CommsCheck;

public readonly record struct TransferedToScotland : IReasonForRemoval
{
    public string Code => "SCT";

    public bool NotSet => false;
    public bool HasCode => true;
    public bool IsEmpty => NotSet;
}

