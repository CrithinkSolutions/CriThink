using System.Net;
using CriThink.Common.Endpoints;
using CriThink.Server.Infrastructure.Data;
using CriThink.Server.Web.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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

        public ServiceController(IWebHostEnvironment env)
        {
            _env = env;
        }

        /// <summary>
        /// Returns the current environment name
        /// </summary>
        /// <returns>Environment name</returns>
        [AllowAnonymous]
        [Route(EndpointConstants.ServiceEnvironment)] // api/service/environment
        [ProducesResponseType(typeof(int), 200)]
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
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(typeof(int), 503)]
        [Produces("application/json")]
        [HttpGet]
        public IActionResult GetRedisHealthStatus()
        {
            var redis = CriThinkRedisMultiplexer.GetConnection();

            var isHealthy = redis.IsConnected;
            return isHealthy ?
                Ok() :
                StatusCode((int) HttpStatusCode.ServiceUnavailable);
        }
    }
}
