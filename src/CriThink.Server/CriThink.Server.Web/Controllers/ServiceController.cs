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

        public ServiceController(IWebHostEnvironment env, CriThinkDbContext dbContext)
        {
            _env = env ?? throw new ArgumentNullException(nameof(env));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
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
                Ok() :
                StatusCode((int) HttpStatusCode.ServiceUnavailable);
        }

        /// <summary>
        /// Returns the SQL Server connection status
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route(EndpointConstants.ServiceSqlServerHealth)] // api/service/sqlserver-health
        [ProducesResponseType(StatusCodes.Status200OK)]
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
                Ok() :
                StatusCode((int) HttpStatusCode.ServiceUnavailable);
        }
    }
}
