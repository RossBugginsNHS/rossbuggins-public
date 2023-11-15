namespace CommsCheck;
public interface IReasonForRemoval
{
    string Code { get; }
    public static Death Death => new Death();
    public static MovedAway MovedAway => new MovedAway();
    public static NoReasonForRemoval None => new NoReasonForRemoval();

   bool IsEmpty();

   bool NotSet {get;}

   public bool HasCode {get;}

    public static IReasonForRemoval FromEnum(RfREnum? e) =>
     e switch
     {
        null => IReasonForRemoval.None,
        RfREnum.DEA => IReasonForRemoval.Death,
        RfREnum.CGA => IReasonForRemoval.MovedAway,
        _ => throw new NotImplementedException()
     };
}
