namespace CommsCheck;
public record NoReasonForRemoval : IReasonForRemoval
{
    public string Code => string.Empty;

    public bool NotSet => true;
    public bool HasCode => false;
    public bool IsEmpty => NotSet;
}