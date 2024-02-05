namespace CommsCheck;

public readonly record struct CommsCheckQuestionRequestDtoCopy(
    DateOnly RelativeDate,
    DateOnly DateOfBirth,
    DateOnly DateOfSmsMostRecentUpdate,
    DateOnly DateOfEmailMostRecentUpdate,
    DateOnly DateOfAppMostRecentUpdate,
    DateOnly DateOfPostalMostRecentUpdate,
    DateOnly DateOfReasonForRemovalMostRecentUpdate,
    DeathStatus? DeathStatusValue,
    ReasonForRemovals? RfR,
    PostalCode PostCode,
    bool SFlag
    )
{
    public static CommsCheckQuestionRequestDtoCopy FromDto(CommsCheckQuestionRequestDto dto) =>
         new CommsCheckQuestionRequestDtoCopy(
            dto.RelativeDate,
              dto.DateOfBirth,
                dto.DateOfSmsMostRecentUpdate,
                dto.DateOfEmailMostRecentUpdate,
                dto.DateOfAppMostRecentUpdate,
                dto.DateOfPostalMostRecentUpdate,
                dto.DateOfReasonForRemovalMostRecentUpdate,
                dto.DeathStatusValue,
                 dto.RfR,
                 dto.PostCode,
                 dto.Flags.SFlag);

    public CommsCheckQuestionRequestDto ToDto() => 
             new CommsCheckQuestionRequestDto(
                RelativeDate,
              DateOfBirth,
                DateOfSmsMostRecentUpdate,
                DateOfEmailMostRecentUpdate,
                DateOfAppMostRecentUpdate,
                DateOfPostalMostRecentUpdate,
                DateOfReasonForRemovalMostRecentUpdate,
                DeathStatusValue,
                RfR,
                PostCode,
                new RecordFlags(SFlag));
}