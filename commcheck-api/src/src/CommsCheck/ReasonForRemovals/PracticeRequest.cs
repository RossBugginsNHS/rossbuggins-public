
namespace CommsCheck;

public readonly record struct PracticeRequest : IReasonForRemoval
{
    public string Code => "RDR";

    public bool NotSet => false;
    public bool HasCode => true;
    public bool IsEmpty => NotSet;
}
