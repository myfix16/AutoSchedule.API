using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoSchedule.Core.Models;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AutoSchedule.API
{
    public class FieldsSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            List<FieldInfo> fields = context.Type.GetFields()
                // ! System.Text.Json.Serialization must be included, otherwise, GetCustomAttribute won't find the attribute.
                .Where(f => f.GetCustomAttribute<System.Text.Json.Serialization.JsonIncludeAttribute>() != null)
                .ToList();

            if (fields.Count == 0) return;

            foreach (FieldInfo field in fields)
            {
                schema.Properties[field.Name] = field.FieldType.Name switch
                {
                    "String" => new OpenApiSchema() { Type = "string" },
                    nameof(Priority) => new OpenApiSchema()
                    {
                        Type = "integer", 
                        Format = "int32", 
                        Enum = new List<IOpenApiAny>()
                        {
                            new OpenApiInteger(0),
                            new OpenApiInteger(1),
                            new OpenApiInteger(2),
                        }
                    },
                    _ => new OpenApiSchema() { Type = field.FieldType.Name }
                };
            }
        }
    }
}