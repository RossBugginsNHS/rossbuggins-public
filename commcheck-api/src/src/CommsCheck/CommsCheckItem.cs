namespace CommsCheck;
public readonly record struct CommsCheckItem(
    DateOnly UtcDateCheckItemCreated,
    DateOnly DateOfBirth,
    DateOnly DateOfSmsMostRecentUpdate,
    DateOnly DateOfEmailMostRecentUpdate,
    DateOnly DateOfAppMostRecentUpdate,
    DateOnly DateOfPostalMostRecentUpdate,
    DateOnly DateOfReasonForRemovalMostRecentUpdate,
    IReasonForRemoval ReasonForRemoval,
    IDeathStatus DeathStatus)
{
    public int DaysOld =>
        UtcDateCheckItemCreated.DayNumber - DateOfBirth.DayNumber;

    public int DaySinceSmsUpdate =>
        UtcDateCheckItemCreated.DayNumber - DateOfSmsMostRecentUpdate.DayNumber;

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
         UtcDateCheckItemCreated.DayNumber - DateOfMostRecentCommsUpdate.DayNumber;

    public int DaysSinceOldestCommsUpdate => 
         UtcDateCheckItemCreated.DayNumber - DateOfOldestCommsUpdate.DayNumber;
}

public class CommsCheckItemFactory(TimeProvider timeProvider)
{
    public CommsCheckItem FromDtoRelativeToToday(CommsCheckQuestionRequestDto dto)
    {
        return new CommsCheckItem(
        DateOnly.FromDateTime(timeProvider.GetUtcNow().DateTime),
        dto.DateOfBirth,
        dto.DateOfSmsMostRecentUpdate,
        dto.DateOfEmailMostRecentUpdate,
        dto.DateOfAppMostRecentUpdate,
        dto.DateOfPostalMostRecentUpdate,
        dto.DateOfReasonForRemovalMostRecentUpdate,
        IReasonForRemoval.FromEnum(dto.RfR),
        IDeathStatus.FromEnum(dto.DeathStatusValue));
    }
}