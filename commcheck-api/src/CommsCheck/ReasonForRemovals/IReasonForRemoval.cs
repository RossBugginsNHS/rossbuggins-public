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

    public static IReasonForRemoval FromEnum(ReasonForRemoval? e) =>
     e switch
     {
        null => IReasonForRemoval.None,
        ReasonForRemoval.DEA => IReasonForRemoval.Death,
        ReasonForRemoval.CGA => IReasonForRemoval.MovedAway,
        _ => throw new NotImplementedException()
     };
}
