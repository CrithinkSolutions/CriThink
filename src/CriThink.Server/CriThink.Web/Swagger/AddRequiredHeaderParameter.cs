using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CriThink.Web.Swagger
{
    public class AddRequiredHeaderParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation == null || context == null) return;

            var apiDescription = context.ApiDescription;
            var apiVersion = apiDescription.GetApiVersion();

            var model = apiDescription.ActionDescriptor.GetApiVersionModel(ApiVersionMapping.Explicit | ApiVersionMapping.Implicit);
            operation.Deprecated = model.DeprecatedApiVersions.Contains(apiVersion);

            operation.Parameters?.Add(new OpenApiParameter
            {
                Name = "api-version",
                In = ParameterLocation.Header,
                AllowEmptyValue = true,
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "String",
                    Default = new OpenApiString(
                        model.DeclaredApiVersions.Any() ?
                            model.DeclaredApiVersions[0]?.ToString() :
                            string.Empty)
                }
            });
        }
    }
}
