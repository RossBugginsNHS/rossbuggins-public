namespace CommsCheck;

public class CommsCheckItemFactory
{
    public CommsCheckItem FromDtoRelativeToToday(CommsCheckQuestionRequestDto dto)
    {
        return new CommsCheckItem(
        dto.RelativeDate,
        dto.DateOfBirth,
        dto.DateOfSmsMostRecentUpdate,
        dto.DateOfEmailMostRecentUpdate,
        dto.DateOfAppMostRecentUpdate,
        dto.DateOfPostalMostRecentUpdate,
        dto.DateOfReasonForRemovalMostRecentUpdate,
        IReasonForRemoval.FromEnum(dto.RfR),
        IDeathStatus.FromEnum(dto.DeathStatusValue),
        dto.PostCode,
        dto.Flags.SFlag,
        CommsCheckQuestionRequestDtoCopy.FromDto(dto));
    }
}
