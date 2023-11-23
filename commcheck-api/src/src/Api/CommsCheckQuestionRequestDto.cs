namespace CommsCheck;

using System.Globalization;
using Swashbuckle.AspNetCore.Annotations;

public readonly record struct CommsCheckQuestionRequestDto(
    [property: SwaggerSchema("Date to base age and duration calculations from")]
    DateOnly RelativeDate,

    [property: SwaggerSchema("Date of birth yyyy-mm-dd")]
    [property: SwaggerSchemaExample("2000-06-07")]
    DateOnly DateOfBirth,
    DateOnly DateOfSmsMostRecentUpdate,
    DateOnly DateOfEmailMostRecentUpdate,
    DateOnly DateOfAppMostRecentUpdate,
    DateOnly DateOfPostalMostRecentUpdate,
    DateOnly DateOfReasonForRemovalMostRecentUpdate,
    DeathStatus? DeathStatusValue,
    ReasonForRemovals? RfR,

    [property: SwaggerSchema("Postcode. Eg AB01 1AB")]
    [property: SwaggerSchemaExample("AB01 1AB")]
    PostalCode PostCode)
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
            PostalCode.Empty);

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
