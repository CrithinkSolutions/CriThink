﻿using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.Json;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
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

#pragma warning disable CA1062 // Validate arguments of public methods

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

#pragma warning restore CA1062 // Validate arguments of public methods

        /// <summary>
        /// Handle the exception thrown
        /// </summary>
        /// <param name="context">The http context</param>
        /// <param name="ex">The exception thrown</param>
        /// <returns></returns>
        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected

            ApiResponse parameter;
            string result = null;

            switch (ex)
            {
                case PostgresException:
                case RetryLimitExceededException:
                case SmtpException:
                    code = HttpStatusCode.ServiceUnavailable;
                    _logger.LogCritical(ex, "An infrastructure component is not available");
                    parameter = new ApiResponse((int) code, "Can't process the request right now");
                    break;
                case Core.Exceptions.IdentityOperationException identityOperationException:
                    code = HttpStatusCode.BadRequest;
                    _logger.LogWarning(identityOperationException, "An identity related operation failed");
                    parameter = new ApiBadRequestResponse(
                        identityOperationException.Resource,
                        identityOperationException.IdentityResult.Errors.Select(e => e.Description));

                    result = JsonSerializer.Serialize((ApiBadRequestResponse) parameter);
                    break;
                case Core.Exceptions.ResourceNotFoundException resourceNotFoundException:
                    code = HttpStatusCode.NotFound;
                    _logger.LogWarning(resourceNotFoundException, "An asked resource has not been found");
                    parameter = new ApiResponse((int) code, ex.Message);
                    break;
                case Core.Exceptions.RefreshTokenExpiredException _:
                    code = HttpStatusCode.Forbidden;
                    parameter = new ApiResponse((int) code, "The given refresh token is invalid or expired");
                    context.Response.Headers.Add(HeadersConstants.RefreshTokenExpired, new StringValues("true"));
                    break;
                default:
                    parameter = new ApiResponse((int) code, "Can't process the request right now");
                    _logger.LogError(ex, "Generic exception");
                    break;
            }

            result ??= JsonSerializer.Serialize(parameter);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) code;
            return context.Response.WriteAsync(result);
        }
    }
}
