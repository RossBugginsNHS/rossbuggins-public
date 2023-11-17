
namespace CommsCheck;

public readonly record struct PracticeRequestImmediateRemoval : IReasonForRemoval
{
    public string Code => "RDI";

    public bool NotSet => false;
    public bool HasCode => true;
    public bool IsEmpty => NotSet;
}
