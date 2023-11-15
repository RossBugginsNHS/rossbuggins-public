namespace CommsCheck;

using System.Text.Json.Serialization;

public readonly record struct CommsCheckQuestionRequestDto(
    DateOnly DateOfBirth, 
    DateOnly DateOfSmsUpdate,
    RfREnum? RfR)
{

}
