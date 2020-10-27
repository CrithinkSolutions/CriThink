﻿using System;
using CriThink.Common.Endpoints;
using CriThink.Server.Web.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
    [Route(EndpointConstants.ApiBase + EndpointConstants.ServiceBase)] //api/service
    public class ServiceController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<ServiceController> _logger;

        public ServiceController(IWebHostEnvironment env, ILogger<ServiceController> logger)
        {
            _env = env ?? throw new ArgumentNullException(nameof(env));
            _logger = logger;
        }

        /// <summary>
        /// Returns the current environment name
        /// </summary>
        /// <returns>Environment name</returns>
        [AllowAnonymous]
        [Route(EndpointConstants.ServiceEnvironment)] // api/service/environment
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpGet]
        public IActionResult GetEnvironment()
        {
            var name = _env.EnvironmentName;
            return Ok($"Environment: {name}");
        }

        /// <summary>
        /// Log sample entries at Information, Critical, Error and Warning levels
        /// </summary>
        [AllowAnonymous]
        [Route(EndpointConstants.ServiceLoggingHealth)] // api/service/logging-health
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpHead]
        public IActionResult LogSampleEntry()
        {
            _logger?.LogInformation("Test log as information");
            _logger?.LogCritical("Test log as critical");
            _logger?.LogError("Test log as error");
            _logger?.LogWarning("Test log as warning");

            return NoContent();
        }

        [AllowAnonymous]
        [Route(EndpointConstants.ServiceEnableSignup)] // api/service/signup-enabled
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public IActionResult GetSignupActivation()
        {
            var enableSignup = Environment.GetEnvironmentVariable("ENABLE_SIGNUP");
            if (enableSignup == null)
                return Ok(false);

            var isEnabled = bool.Parse(enableSignup);
            return Ok(isEnabled);
        }
    }
}
