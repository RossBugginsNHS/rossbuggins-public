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
    IDeathStatus DeathStatus,
    PostalCode PostCode,
    CommsCheckQuestionRequestDtoCopy CopyOfSource)
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
        DateOnly.FromDateTime(timeProvider.GetUtcNow().UtcDateTime),
        dto.DateOfBirth,
        dto.DateOfSmsMostRecentUpdate,
        dto.DateOfEmailMostRecentUpdate,
        dto.DateOfAppMostRecentUpdate,
        dto.DateOfPostalMostRecentUpdate,
        dto.DateOfReasonForRemovalMostRecentUpdate,
        IReasonForRemoval.FromEnum(dto.RfR),
        IDeathStatus.FromEnum(dto.DeathStatusValue),
        dto.PostCode,
        CommsCheckQuestionRequestDtoCopy.FromDto(dto));
    }
}

public readonly record struct CommsCheckQuestionRequestDtoCopy(DateOnly DateOfBirth,
    DateOnly DateOfSmsMostRecentUpdate,
    DateOnly DateOfEmailMostRecentUpdate,
    DateOnly DateOfAppMostRecentUpdate,
    DateOnly DateOfPostalMostRecentUpdate,
    DateOnly DateOfReasonForRemovalMostRecentUpdate,
    DeathStatus? DeathStatusValue,
    ReasonForRemovals? RfR,
    PostalCode PostCode
    )
{
    public static CommsCheckQuestionRequestDtoCopy FromDto(CommsCheckQuestionRequestDto dto) =>
         new CommsCheckQuestionRequestDtoCopy(
              dto.DateOfBirth,
                dto.DateOfSmsMostRecentUpdate,
                dto.DateOfEmailMostRecentUpdate,
                dto.DateOfAppMostRecentUpdate,
                dto.DateOfPostalMostRecentUpdate,
                dto.DateOfReasonForRemovalMostRecentUpdate,
                dto.DeathStatusValue,
                 dto.RfR,
                 dto.PostCode);

    public CommsCheckQuestionRequestDto ToDto() => 
             new CommsCheckQuestionRequestDto(
              DateOfBirth,
                DateOfSmsMostRecentUpdate,
                DateOfEmailMostRecentUpdate,
                DateOfAppMostRecentUpdate,
                DateOfPostalMostRecentUpdate,
                DateOfReasonForRemovalMostRecentUpdate,
                DeathStatusValue,
                RfR,
                PostCode);
}