using System;
using System.Net.Mime;
using CriThink.Common.Endpoints;
using CriThink.Server.Web.ActionFilters;
using CriThink.Server.Web.Models.DTOs;
using CriThink.Server.Web.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Web.Controllers
{
    /// <summary>
    /// This controller contains APIs to give information about the service health
    /// </summary>
    [ApiVersion(EndpointConstants.VersionOne)]
    [ApiController]
    [ApiValidationFilter]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route(EndpointConstants.ApiBase + EndpointConstants.ServiceBase)]
    public class ServiceController : ControllerBase
    {
        private readonly IAppVersionService _appService;
        private readonly ILogger<ServiceController> _logger;

        public ServiceController(
            IAppVersionService appService,
            ILogger<ServiceController> logger)
        {
            _appService = appService ?? throw new ArgumentNullException(nameof(appService));
            _logger = logger;
        }

        /// <summary>
        /// Returns the current environment name
        /// </summary>
        /// <remarks> 
        /// Sample request:
        /// 
        ///     GET: /api/service/environment
        /// 
        /// </remarks>
        /// <response code="200">Returns enivronment name</response>
        /// <response code="500">If the server can't process the request</response>
        [AllowAnonymous]
        [Route(EndpointConstants.ServiceEnvironment)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Text.Plain)]
        [HttpGet]
        public IActionResult GetEnvironment()
        {
            var name = _appService.CurrentEnvironment;
            return Ok($"Environment: {name}");
        }

        /// <summary>
        /// Log sample entries at Information, Critical, Error and Warning levels
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     HEAD: /api/service/logging-health
        /// 
        /// </remarks>
        /// <response code="204">Returns when operation succeeds</response>
        /// <response code="500">If the server can't process the request</response>
        [AllowAnonymous]
        [Route(EndpointConstants.ServiceLoggingHealth)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [HttpHead]
        public IActionResult LogSampleEntry()
        {
            _logger?.LogInformation("Test log as information");
            _logger?.LogCritical("Test log as critical");
            _logger?.LogError("Test log as error");
            _logger?.LogWarning("Test log as warning");

            return NoContent();
        }

        /// <summary>
        /// Returns a flag indicating whatever the mobile app should work or not
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     HEAD: /api/service/app-enabled
        /// 
        /// </remarks>
        /// <response code="204">Returns when the app should work</response>
        /// <response code="403">Returns when the app should not work</response>
        /// <response code="500">If the server can't process the request</response>
        [AllowAnonymous]
        [Route(EndpointConstants.ServiceAppEnabled)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [HttpHead]
        public IActionResult IsAppEnabled()
        {
            var isParsed = bool.TryParse(Environment.GetEnvironmentVariable("IS_APP_ENABLED"), out var isEnabled);
            if (isParsed)
                return isEnabled ?
                    NoContent() :
                    StatusCode(403);

            return StatusCode(500);
        }
    }
}
