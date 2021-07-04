using System;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Common.Endpoints.DTOs.Statistics;
using CriThink.Server.Core.Interfaces;
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
    [Consumes("application/json")]
    [Produces("application/json")]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService ?? throw new ArgumentNullException(nameof(statisticsService));
        }

        /// <summary>
        /// Returns the users total counting
        /// </summary>
        /// <remarks> 
        /// Sample request:
        /// 
        ///     GET: /api/searches/counting
        /// 
        /// </remarks>
        /// <response code="200">Returns total number of users</response>
        /// <response code="403">If the givne refresh token is invalid or expired</response>
        /// <response code="500">If the server can't process the request</response>
        /// <response code="503">If the server is not ready to handle the request</response>
        [ResponseCache(Duration = 3600)]
        [Route(EndpointConstants.StatisticsUsersCounting)]
        [ProducesResponseType(typeof(UsersCountingResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status503ServiceUnavailable)]
        [HttpGet]
        public async Task<IActionResult> GetUsersCountingAsync()
        {
            var users = await _statisticsService.GetUsersCountingAsync();
            return Ok(new ApiOkResponse(users));
        }

        /// <summary>
        /// Returns the searches total counting
        /// </summary>
        /// <remarks> 
        /// Sample request:
        /// 
        ///     GET: /api/searches/counting
        /// 
        /// </remarks>
        /// <response code="200">Returns total number of users</response>
        /// <response code="403">If the givne refresh token is invalid or expired</response>
        /// <response code="500">If the server can't process the request</response>
        /// <response code="503">If the server is not ready to handle the request</response>
        [ResponseCache(Duration = 3600)]
        [Route(EndpointConstants.StatisticsSearches)]
        [ProducesResponseType(typeof(SearchesCountingResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status503ServiceUnavailable)]
        [HttpGet]
        public async Task<IActionResult> GetTotalSearchesAsync()
        {
            var totalSearches = await _statisticsService.GetTotalSearchesAsync();
            return Ok(new ApiOkResponse(totalSearches));
        }
    }
}
