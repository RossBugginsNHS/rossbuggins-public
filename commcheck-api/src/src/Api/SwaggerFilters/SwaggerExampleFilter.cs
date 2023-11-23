namespace CommsCheck;

using System.Reflection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;

public class SwaggerExampleFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.MemberInfo != null)
        {
            {
                var schemaAttribute = context.MemberInfo.GetCustomAttributes<SwaggerSchemaExampleAttribute>().FirstOrDefault();
                if (schemaAttribute != null)
                {
                    schema.Example = new OpenApiString(schemaAttribute.Example);
                }
            }
            {
                var schemaAttribute = context.MemberInfo.GetCustomAttributes<SwaggerSchemaAttribute>().FirstOrDefault();
                if (schemaAttribute != null)
                {
                    schema.Description = schemaAttribute.Description;
                }
            }
        }
    }
}
