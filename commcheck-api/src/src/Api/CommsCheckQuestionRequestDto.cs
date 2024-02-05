namespace CommsCheck;

using System.Globalization;
using Swashbuckle.AspNetCore.Annotations;

public readonly record struct CommsCheckQuestionRequestDto(
    [property: SwaggerSchema("Date to base age and duration calculations from")]
    DateOnly RelativeDate,

    [property: SwaggerSchema("Date of birth yyyy-mm-dd")]
    [property: SwaggerSchemaExample("2000-06-07")]
    DateOnly DateOfBirth,

    [property: SwaggerSchemaExample("2022-01-01")]
    DateOnly DateOfSmsMostRecentUpdate,

    [property: SwaggerSchemaExample("2022-06-01")]
    DateOnly DateOfEmailMostRecentUpdate,

    [property: SwaggerSchemaExample("2022-07-01")]
    DateOnly DateOfAppMostRecentUpdate,

    [property: SwaggerSchema("Date postal address was last updated. yyyy-mm-dd")]
    [property: SwaggerSchemaExample("2021-01-01")]
    DateOnly DateOfPostalMostRecentUpdate,

    [property: SwaggerSchemaExample("2023-01-01")]
    DateOnly DateOfReasonForRemovalMostRecentUpdate,
    
    DeathStatus? DeathStatusValue,
    ReasonForRemovals? RfR,

    [property: SwaggerSchema("Postcode. Eg AB01 1AB")]
    [property: SwaggerSchemaExample("AB01 1AB")]
    PostalCode PostCode,
    
    [property: SwaggerSchema("NHS Flags.")]
    RecordFlags Flags)
{

    public static CommsCheckQuestionRequestDto DobOnly(
        DateOnly relativeDate,
        DateOnly dateOfBirth,
        ReasonForRemovals? reasonForRemoval) =>
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
            PostalCode.Empty,
            RecordFlags.Empty);

    public CommsCheckQuestionRequestDto FilteredVersion() =>
        this with 
            {
                DateOfBirth = new DateOnly(
                    this.DateOfBirth.Year,
                    this.DateOfBirth.Month,
                    1),
                PostCode = this.PostCode.DistrictOnly()
            };
}

public readonly record struct RecordFlags(
    [property: SwaggerSchema("NHS S Flag")]
    bool SFlag)
    {

    public static RecordFlags Empty => new RecordFlags();
    }