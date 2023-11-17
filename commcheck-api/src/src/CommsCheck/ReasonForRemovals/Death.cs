
namespace CommsCheck;
public readonly record struct Death : IReasonForRemoval
{
    public string Code => "DEA";

    public bool NotSet => false;
    public bool HasCode => true;
    public bool IsEmpty() => NotSet;
}
