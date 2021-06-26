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
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService ?? throw new ArgumentNullException(nameof(statisticsService));
        }

        [ResponseCache(Duration = 3600)]
        [Route(EndpointConstants.StatisticsUsersCounting)]
        [ProducesResponseType(typeof(UsersCountingResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status503ServiceUnavailable)]
        [Produces("application/json")]
        [HttpGet]
        public async Task<IActionResult> GetUsersCountingAsync()
        {
            var users = await _statisticsService.GetUsersCountingAsync();
            return Ok(new ApiOkResponse(users));
        }
    }
}
