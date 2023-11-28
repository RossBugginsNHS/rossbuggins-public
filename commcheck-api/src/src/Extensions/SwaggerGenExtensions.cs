namespace CommsCheck;

using System.Reflection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public static class SwaggerGenExtensions
{
    public static string OpenApiDescription = """
        Rules engine for providing a standardised and central decision tree for deciding 
        if there should be communication with a person.

        This API makes its decision based on the data provided in the request payload, and does
        not use data sources directly.

        - Submit data using the POST /check endpoint
        - The response contains a Location header with a url and unique id
        - Retrieve answer from GET /check/results/{resultId} endpoint

        Available at the readme github repo is source for a cli tool for directly calling the api.

        - GET <a href="/check/results">/check/results</a> provides a list of the last 100 queries
        - GET <a href="/check/results/stream">/check/results/stream</a> can be used to provide a realtime stream of requests.

        Notes for data payload:
        - Relative Date - this is used to calculate any date spans, for example Age based on Date of birth. 
        - Dob will be always set to first day of the month.
        - Postcode will be set to first 4 characters of postcode.

        Notes for API:
        - The api is deterministic. It will always give the same result based on input data (as long as the rule set is the same)
        - Metrics for the API can be found at GET <a href="/metrics">/metrics</a> endpoint.
        - View current rules at GET <a href="/rules">/rules</a> endpoint.

        Response Data:
        - resultId: The unique hash for the request. Based on the payload of the provided request.
        - request: The data provided for the request.
        - response: The output decision from the rules for each communication type.
        - item: The data object the rules run
        - rulesFileHash: Hash of the rule file - the version
        - ruleOutcomes: The outcome of each rule that was run on the item.

        How the rules work. For each communication type, the rules are evaluated in the order: 
        - 1) Explicit Block All rules are run first. These will block all communication types. For example, reason for removal of DEA.
        - 2) Explicit Block for that communication type. For example - ZZ99 post code blocks postal.
        - 3) Allow rules for that communication type.
        - 4) If no allow rules succeed, then the default is to block.

        Example Client:
        - <a href="https://salmon-water-09aab3203.4.azurestaticapps.net">Example Blazor App</a>
        """;

    public static SwaggerGenOptions AddDateAndEnumFormatters(
        this SwaggerGenOptions options)
    {
        options.SchemaFilter<SwaggerExampleFilter>();
        
        options.MapType<DateOnly>(() => new OpenApiSchema()
        {
            Type = "string",
            Format = "date"
        });

        options.MapType<PostalCode>(() => new OpenApiSchema()
        {
            Type = "string", 
            Default = new OpenApiString("None"), 
            Example = new OpenApiString("None")
        });


        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        options.IncludeXmlComments(xmlPath);
        options.SchemaFilter<EnumTypesSchemaFilter>(xmlPath);

        return options;
    }
}
