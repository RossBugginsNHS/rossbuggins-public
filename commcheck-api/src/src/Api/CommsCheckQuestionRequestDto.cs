namespace CommsCheck;

using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

 public class PostalCodeJsonConverter : JsonConverter<PostalCode>
    {
        public override PostalCode Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options) =>
                PostalCode.Parse(reader.GetString()!);

        public override void Write(
            Utf8JsonWriter writer,
            PostalCode postCodeValue,
            JsonSerializerOptions options) =>
                writer.WriteStringValue(postCodeValue.PostCode);
    }

public readonly record struct PostalCode 
{
    public string PostCode {get;init;}

    public bool IsZZ99 => PostCode!=null && PostCode.StartsWith("ZZ99");

    [JsonConstructor]
    public PostalCode(string postCode)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(postCode, nameof(postCode));
        PostCode = ParsePostcodeString(postCode);
    }

    public static PostalCode Parse(string postCode)
    {
        return new PostalCode(postCode);
    }

    private static string ParsePostcodeString(string postcode)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(postcode, nameof(postcode));
        return postcode.ToUpper().Replace(" ", "");
    }

    public static PostalCode Empty =>  new PostalCode();
}

public readonly record struct CommsCheckQuestionRequestDto(
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
            reasonForRemoval,
            PostalCode.Empty);
}
