namespace CommsCheck;

using System.Text.Json.Serialization;

public readonly record struct CommsCheckQuestionRequestDto(
    int IntCheck,
    DateOnly DateOfBirth, 
    DateOnly DateOfSmsMostRecentUpdate,
    DateOnly DateOfEmailMostRecentUpdate,
    DateOnly DateOfAppMostRecentUpdate,
    DateOnly DateOfPostalMostRecentUpdate,
    DateOnly DateOfReasonForRemovalMostRecentUpdate,
    DeathStatus? DeathStatusValue,
    ReasonForRemovals? RfR)
{

    public static CommsCheckQuestionRequestDto DobOnly(
        DateOnly dateOfBirth, 
        ReasonForRemovals?reasonForRemoval) =>
        new CommsCheckQuestionRequestDto(
            0,
            dateOfBirth,
            DateOnly.MinValue,
            DateOnly.MinValue,
            DateOnly.MinValue,
            DateOnly.MinValue,
            DateOnly.MinValue,
            DeathStatus.None,
            reasonForRemoval);
}
