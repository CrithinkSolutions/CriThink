using System;
using System.Net.Mime;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Common.Endpoints.DTOs.Statistics;
using CriThink.Server.Application.Queries;
using CriThink.Server.Infrastructure.ExtensionMethods;
using CriThink.Server.Web.Models.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CriThink.Server.Web.Controllers
{
    /// <summary>
    /// This controller contains APIs to get public usage statistics
    /// </summary>
    [ApiVersion(EndpointConstants.VersionOne)]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route(EndpointConstants.ApiBase + EndpointConstants.StatisticsBase)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsQueries _statisticsQueries;

        public StatisticsController(IStatisticsQueries statisticsQueries)
        {
            _statisticsQueries = statisticsQueries ?? throw new ArgumentNullException(nameof(statisticsQueries));
        }

        /// <summary>
        /// Returns platform statistics usage
        /// </summary>
        /// <remarks> 
        /// Sample request:
        /// 
        ///     GET: /api/statistics/platform
        /// 
        /// </remarks>
        /// <response code="200">Returns the total number of users and searches</response>
        /// <response code="403">If the givne refresh token is invalid or expired</response>
        /// <response code="500">If the server can't process the request</response>
        /// <response code="503">If the server is not ready to handle the request</response>
        [ResponseCache(Duration = 3600)]
        [Route(EndpointConstants.StatisticsPlatform)]
        [ProducesResponseType(typeof(PlatformDataUsageResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status503ServiceUnavailable)]
        [HttpGet]
        public async Task<IActionResult> GetPlatformUsageDataAsync()
        {
            var users = await _statisticsQueries.GetPlatformUsageDataAsync();
            return Ok(new ApiOkResponse(users));
        }

        /// <summary>
        /// Returns the user searches total counting
        /// </summary>
        /// <remarks> 
        /// Sample request:
        /// 
        ///     GET: /api/statistics/user
        /// 
        /// </remarks>
        /// <response code="200">Returns total number of searches performed by user</response>
        /// <response code="403">If the givne refresh token is invalid or expired</response>
        /// <response code="500">If the server can't process the request</response>
        /// <response code="503">If the server is not ready to handle the request</response>
        [Route(EndpointConstants.StatisticsUserSearches)]
        [ProducesResponseType(typeof(SearchesCountingResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status503ServiceUnavailable)]
        [HttpGet]
        public async Task<IActionResult> GetTotalUserSearchesAsync()
        {
            var userId = User.GetId();

            var userTotalSearches = await _statisticsQueries.GetTotalSearchesByUserIdAsync(userId);
            return Ok(new ApiOkResponse(userTotalSearches));
        }
    }
}
