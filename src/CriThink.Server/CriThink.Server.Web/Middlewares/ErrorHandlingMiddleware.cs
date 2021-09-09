using System;
using System.Linq;
using System.Net.Mail;
using System.Text.Json;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Server.Core.Exceptions;
using CriThink.Server.Web.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Npgsql;

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
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
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
            ApiResponse apiResponse;

            if (ex is CriThinkBaseException baseException)
            {
                apiResponse = ManageCriThinkExceptions(context, baseException);
            }
            else if (ex is CriThinkSecurityException)
            {
                apiResponse = ManageSecurityExceptions(context);
            }
            else if (ex is CriThinkNotFoundException notFoundException)
            {
                apiResponse = ManageNotFoundExceptions(context, notFoundException);
            }
            else if (ex is CriThinkIdentityOperationException identityException)
            {
                apiResponse = ManageIdentityException(context, identityException);
            }
            else if (ex is PostgresException ||
                     ex is RetryLimitExceededException ||
                     ex is SmtpException)
            {
                apiResponse = ManageInfrastructureException(context, ex);
            }
            else
            {
                apiResponse = ManageGenericExceptions(context, ex);
            }

            var result = JsonSerializer.Serialize(apiResponse);

            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync(result);
        }

        private static ApiResponse ManageSecurityExceptions(
            HttpContext context)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Response.Headers.Add(HeadersConstants.RefreshTokenExpired, new StringValues("true"));
            return new ApiResponse(StatusCodes.Status403Forbidden, "The given tokens are invalid or expired");
        }

        private ApiResponse ManageGenericExceptions(
            HttpContext context,
            Exception exception)
        {
            _logger?.LogCritical(exception, "Unhandled exception");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            return new ApiResponse(StatusCodes.Status500InternalServerError);
        }

        private ApiResponse ManageNotFoundExceptions(
            HttpContext context,
            CriThinkNotFoundException exception)
        {
            _logger?.LogWarning(exception, "An asked resource has not been found");
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            return new ApiResponse(StatusCodes.Status404NotFound, exception.Message);
        }

        private ApiResponse ManageIdentityException(
            HttpContext context,
            CriThinkIdentityOperationException exception)
        {
            _logger.LogWarning(exception, "An identity related operation failed");
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return new ApiBadRequestResponse(
                exception.Resource,
                exception.IdentityResult.Errors.Select(e => e.Description));
        }

        private ApiResponse ManageInfrastructureException(
            HttpContext context,
            Exception ex)
        {
            _logger.LogCritical(ex, "An infrastructure component is not available");
            context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            return new ApiResponse(StatusCodes.Status503ServiceUnavailable, "We can't process the request right now");
        }

        private ApiResponse ManageCriThinkExceptions(
            HttpContext context,
            CriThinkBaseException exception)
        {
            _logger?.LogWarning(exception, "Handled exception");
            context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
            return new ApiResponse(exception.Code, exception.Message);
        }
    }
}
