
namespace CommsCheck;

public readonly record struct FromResidentialInsitute : IReasonForRemoval
{
    public string Code => "RFI";

    public bool NotSet => false;
    public bool HasCode => true;
    public bool IsEmpty => NotSet;
}
