using System;
using System.Net;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Server.Infrastructure.Data;
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
        private readonly CriThinkDbContext _dbContext;
        private readonly ILogger<ServiceController> _logger;

        public ServiceController(IWebHostEnvironment env, CriThinkDbContext dbContext, ILogger<ServiceController> logger)
        {
            _env = env ?? throw new ArgumentNullException(nameof(env));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
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
        /// Returns the Redis connection status
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route(EndpointConstants.ServiceRedisHealth)] // api/service/redis-health
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [HttpHead]
        public IActionResult GetRedisHealthStatus()
        {
            bool isHealthy;

            try
            {
                var redis = CriThinkRedisMultiplexer.GetConnection();
                isHealthy = redis.IsConnected;
            }
            catch (Exception)
            {
                isHealthy = false;
            }

            return isHealthy ?
                NoContent() :
                StatusCode((int) HttpStatusCode.ServiceUnavailable);
        }

        /// <summary>
        /// Returns the SQL Server connection status
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route(EndpointConstants.ServiceSqlServerHealth)] // api/service/sqlserver-health
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [HttpHead]
        public async Task<IActionResult> GetSqlServerHealthStatusAsync()
        {
            bool isHealthy;

            try
            {
                isHealthy = await _dbContext.Database.CanConnectAsync().ConfigureAwait(false);
            }
            catch (Exception)
            {
                isHealthy = false;
            }

            return isHealthy ?
                NoContent() :
                StatusCode((int) HttpStatusCode.ServiceUnavailable);
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
