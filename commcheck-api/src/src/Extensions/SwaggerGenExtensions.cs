namespace CommsCheck;

using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public static class SwaggerGenExtensions
{
    public static SwaggerGenOptions AddDateAndEnumFormatters(
        this SwaggerGenOptions options)
    {
        options.MapType<DateOnly>(() => new OpenApiSchema()
        {
            Type = "string",
            Format = "date"
        });

        options.MapType<PostalCode>(() => new OpenApiSchema()
        {
            Type = "string"
        });


        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        options.IncludeXmlComments(xmlPath);
        options.SchemaFilter<EnumTypesSchemaFilter>(xmlPath);

        return options;
    }
}
