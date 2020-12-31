using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

#pragma warning disable CA1062 // Validate arguments of public methods
namespace CriThink.Server.Web.ActionFilters
{
    public class CrossServiceAuthenticationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var requestedServices = context.HttpContext.RequestServices;

            var logger = ResolveService<ILogger<CrossServiceAuthenticationFilter>>(requestedServices);

            var configuration = ResolveService<IConfiguration>(requestedServices);
            var headerKey = configuration?["CrossService:Header"];
            var headerValue = configuration?["CrossService:Value"];
            if (configuration == null ||
                string.IsNullOrWhiteSpace(headerKey) ||
                string.IsNullOrWhiteSpace(headerValue))
            {
                context.Result = Get401StatusCode();
                logger?.LogCritical("Can't CrossService header values", configuration, headerKey, headerValue);
                return;
            }

            var httpContext = context.HttpContext;
            if (!httpContext.Request.Headers.ContainsKey(headerKey) || httpContext.Request.Headers[headerKey] != headerValue)
            {
                context.Result = Get401StatusCode();
                logger?.LogInformation("The current request does not contain a valid header", httpContext.Request.Headers);
                return;
            }

            base.OnActionExecuting(context);
        }

        private static TType ResolveService<TType>(IServiceProvider serviceProvider) where TType : class =>
            serviceProvider.GetService(typeof(TType)) as TType;

        private static StatusCodeResult Get401StatusCode() =>
            new StatusCodeResult(StatusCodes.Status401Unauthorized);
    }
}
