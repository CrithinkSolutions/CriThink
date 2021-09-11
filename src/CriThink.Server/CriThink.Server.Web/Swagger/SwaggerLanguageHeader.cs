using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CriThink.Server.Web.Swagger
{
    public class SwaggerLanguageHeader : IOperationFilter
    {
        private readonly IServiceProvider _serviceProvider;

        public SwaggerLanguageHeader(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation?.Parameters is null)
                operation.Parameters = new List<OpenApiParameter>();

            var locService = _serviceProvider.GetService(typeof(IOptions<RequestLocalizationOptions>)) as IOptions<RequestLocalizationOptions>;
            var enums = locService.Value?.SupportedCultures?
                .Select(c => new OpenApiString(c.TwoLetterISOLanguageName))
                .Cast<IOpenApiAny>()
                .ToList();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Accept-Language",
                In = ParameterLocation.Header,
                Schema = new OpenApiSchema
                {
                    Type = "String",
                    Default = enums[0],
                    Enum = enums,
                },
                Description = "Supported languages",
                Required = false
            });
        }
    }
}
