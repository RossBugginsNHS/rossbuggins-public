
namespace CommsCheck;

public readonly record struct OutOfPracticeArea : IReasonForRemoval
{
    public string Code => "OPA";

    public bool NotSet => false;
    public bool HasCode => true;
    public bool IsEmpty => NotSet;
}
