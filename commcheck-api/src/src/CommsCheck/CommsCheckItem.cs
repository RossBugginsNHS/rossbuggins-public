namespace CommsCheck;
public readonly record struct CommsCheckItem(
    DateOnly RelativeDate,
    DateOnly DateOfBirth,
    DateOnly DateOfSmsMostRecentUpdate,
    DateOnly DateOfEmailMostRecentUpdate,
    DateOnly DateOfAppMostRecentUpdate,
    DateOnly DateOfPostalMostRecentUpdate,
    DateOnly DateOfReasonForRemovalMostRecentUpdate,
    IReasonForRemoval ReasonForRemoval,
    IDeathStatus DeathStatus,
    PostalCode PostCode,
    bool SFlag,
    CommsCheckQuestionRequestDtoCopy CopyOfSource)
{
    public int DaysOld =>
        RelativeDate.DayNumber - DateOfBirth.DayNumber;

    public int DaySinceSmsUpdate =>
        RelativeDate.DayNumber - DateOfSmsMostRecentUpdate.DayNumber;

    public DateOnly DateOfMostRecentCommsUpdate =>
        Enumerable.Max(
            new DateOnly[]{
            DateOfSmsMostRecentUpdate,
            DateOfEmailMostRecentUpdate,
            DateOfAppMostRecentUpdate,
            DateOfPostalMostRecentUpdate});

    public DateOnly DateOfOldestCommsUpdate =>
        Enumerable.Min(
            new DateOnly[]{
            DateOfSmsMostRecentUpdate,
            DateOfEmailMostRecentUpdate,
            DateOfAppMostRecentUpdate,
            DateOfPostalMostRecentUpdate});

    public int YearsOld => (int)Math.Floor(DaysOld / (float)365);

    public int DaysSinceMostRecentCommsUpdate =>
         RelativeDate.DayNumber - DateOfMostRecentCommsUpdate.DayNumber;

    public int DaysSinceOldestCommsUpdate =>
         RelativeDate.DayNumber - DateOfOldestCommsUpdate.DayNumber;
}
