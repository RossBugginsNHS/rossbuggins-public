
namespace CommsCheck;

public readonly record struct Embarkation : IReasonForRemoval
{
    public string Code => "EMB";
    public bool NotSet => false;
    public bool HasCode => true;
    public bool IsEmpty => NotSet;
}
