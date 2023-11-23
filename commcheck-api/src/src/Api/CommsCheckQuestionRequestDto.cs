namespace CommsCheck;

using System.Globalization;

public readonly record struct CommsCheckQuestionRequestDto(
    DateOnly RelativeDate,
    DateOnly DateOfBirth, 
    DateOnly DateOfSmsMostRecentUpdate,
    DateOnly DateOfEmailMostRecentUpdate,
    DateOnly DateOfAppMostRecentUpdate,
    DateOnly DateOfPostalMostRecentUpdate,
    DateOnly DateOfReasonForRemovalMostRecentUpdate,
    DeathStatus? DeathStatusValue,
    ReasonForRemovals? RfR,
    PostalCode PostCode)
{

    public static CommsCheckQuestionRequestDto DobOnly(
        DateOnly relativeDate,
        DateOnly dateOfBirth, 
        ReasonForRemovals?reasonForRemoval) =>
        new CommsCheckQuestionRequestDto(
            relativeDate,
            dateOfBirth,
            DateOnly.MinValue,
            DateOnly.MinValue,
            DateOnly.MinValue,
            DateOnly.MinValue,
            DateOnly.MinValue,
            DeathStatus.None,
            reasonForRemoval,
            PostalCode.Empty);
}
