public interface IReasonForRemoval
{
    string Code { get; }
    public static Death Death => new Death();
    public static MovedAway MovedAway => new MovedAway();
    public static NoReasonForRemoval None => new NoReasonForRemoval();

    public bool IsEmpty() => this is NoReasonForRemoval;

    public bool NotSet => this is NoReasonForRemoval;

    public bool HasCode => !( this is NoReasonForRemoval);

    public static IReasonForRemoval FromEnum(RfREnum? e) =>
     e switch
     {
        null => IReasonForRemoval.None,
        RfREnum.DEA => IReasonForRemoval.Death,
        RfREnum.CGA => IReasonForRemoval.MovedAway,
        _ => throw new NotImplementedException()
     };
}
