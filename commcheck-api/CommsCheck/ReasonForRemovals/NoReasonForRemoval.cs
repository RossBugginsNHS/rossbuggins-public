namespace CommsCheck;
public record NoReasonForRemoval : IReasonForRemoval
{
    public string Code => string.Empty;
}