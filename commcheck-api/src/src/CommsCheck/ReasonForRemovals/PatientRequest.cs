
namespace CommsCheck;

public readonly record struct PatientRequest : IReasonForRemoval
{
    public string Code => "RPR";

    public bool NotSet => false;
    public bool HasCode => true;
    public bool IsEmpty => NotSet;
}
