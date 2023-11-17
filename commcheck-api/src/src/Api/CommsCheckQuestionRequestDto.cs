namespace CommsCheck;

using System.Text.Json.Serialization;

public readonly record struct CommsCheckQuestionRequestDto(
    DateOnly DateOfBirth, 
    DateOnly DateOfSmsUpdate,
    DateOnly DateOfEmailUpdate,
    DateOnly DateOfAppUpdate,
    DateOnly DateOfPostalUpdate,
    DateOnly DateOfReasonForRemovalUpdate,
    DeathStatus? DeathStatusValue,
    ReasonForRemovals? RfR)
{

    public static CommsCheckQuestionRequestDto DobOnly(
        DateOnly dateOfBirth, 
        ReasonForRemovals?reasonForRemoval) =>
        new CommsCheckQuestionRequestDto(
            dateOfBirth,
            DateOnly.MinValue,
            DateOnly.MinValue,
            DateOnly.MinValue,
            DateOnly.MinValue,
            DateOnly.MinValue,
            DeathStatus.None,
            reasonForRemoval);
}
