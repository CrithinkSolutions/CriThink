using System;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Web.Middlewares
{
    /// <summary>
    /// Response middleware for failed requests
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="next">The request processing</param>
        /// <param name="logger">Logger instance</param>
        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Wrap the action call in a try/catch block to have a standard response in case of error
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Handle the exception thrown
        /// </summary>
        /// <param name="context">The http context</param>
        /// <param name="ex">The exception thrown</param>
        /// <returns></returns>
        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected

            object parameter;

            switch (ex)
            {
                case AggregateException aggregate:
                    code = HttpStatusCode.BadRequest;
                    parameter = new { error = aggregate.InnerExceptions.Select(e => e.Message) };
                    _logger.LogError(aggregate, "Aggregate exception");
                    break;
                default:
                    parameter = new { error = ex.Message };
                    _logger.LogError(ex, "Generic exception");
                    break;
            }

            var result = JsonSerializer.Serialize(parameter);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) code;
            return context.Response.WriteAsync(result);
        }
    }
}
