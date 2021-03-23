using System;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Common.Endpoints.DTOs.Admin;
using CriThink.Server.Core.Interfaces;
using CriThink.Server.Web.ActionFilters;
using CriThink.Server.Web.Models.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CriThink.Server.Web.Controllers
{
    /// <summary>
    /// This controller contains APIs to update the debunking news repository
    /// </summary>
    [ApiVersion(EndpointConstants.VersionOne)]
    [ApiValidationFilter]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route(EndpointConstants.ApiBase + EndpointConstants.DebunkNewsBase)]
    public class DebunkingNewsController : Controller
    {
        private readonly IDebunkingNewsService _debunkingNewsService;

        public DebunkingNewsController(IDebunkingNewsService debunkNewsService)
        {
            _debunkingNewsService = debunkNewsService ?? throw new ArgumentNullException(nameof(debunkNewsService));
        }

        /// <summary>
        /// Trigger the update of internal debunking news repository
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST: /api/debunking-news/trigger-update
        /// 
        /// </remarks>
        /// <response code="204">Returns when operation succeeds</response>
        /// <response code="401">If the user is not authorized</response>
        /// <response code="500">If the server can't process the request</response>
        /// <response code="503">If the server is not ready to handle the request</response>
        [AllowAnonymous]
        [ServiceFilter(typeof(DebunkingNewsTriggerAuthenticationFilter), Order = int.MinValue)]
        [Route(EndpointConstants.DebunkNewsTriggerUpdate)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status503ServiceUnavailable)]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> TriggerRepositoryUpdateAsync()
        {
            await _debunkingNewsService.UpdateRepositoryAsync().ConfigureAwait(false);
            return NoContent();
        }

        /// <summary>
        /// Get all the debunking news
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET: /api/debunking-news/all?{pageSize}{pageIndex}
        /// 
        /// </remarks>
        /// <param name="request">Page index and number of debunking news per page</param>
        /// <response code="200">Returns the list of debunking news</response>
        /// <response code="401">If the user is not authorized</response>
        /// <response code="500">If the server can't process the request</response>
        /// <response code="503">If the server is not ready to handle the request</response>
        [Route(EndpointConstants.DebunkingNewsGetAll)]
        [ProducesResponseType(typeof(DebunkingNewsGetAllResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status503ServiceUnavailable)]
        [Produces("application/json")]
        [HttpGet]
        public async Task<IActionResult> GetAllDebunkingNewsAsync([FromQuery] DebunkingNewsGetAllRequest request)
        {
            var allDebunkingNews = await _debunkingNewsService.GetAllDebunkingNewsAsync(request).ConfigureAwait(false);
            return Ok(new ApiOkResponse(allDebunkingNews));
        }

        /// <summary>
        /// Get the specified debunking news
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET: /api/debunking-news?{id}
        /// 
        /// </remarks>
        /// <param name="request">Debunking news id</param>
        /// <response code="200">Returns the requested debunking news</response>
        /// <response code="400">If the request query string is invalid</response>
        /// <response code="401">If the user is not authorized</response>
        /// <response code="404">If the given id is not found</response>
        /// <response code="500">If the server can't process the request</response>
        /// <response code="503">If the server is not ready to handle the request</response>
        [ProducesResponseType(typeof(DebunkingNewsGetDetailsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiBadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status503ServiceUnavailable)]
        [Produces("application/json")]
        [HttpGet]
        public async Task<IActionResult> GetDebunkingNewsAsync([FromQuery] DebunkingNewsGetRequest request)
        {
            var debunkingNews = await _debunkingNewsService.GetDebunkingNewsAsync(request).ConfigureAwait(false);
            return Ok(new ApiOkResponse(debunkingNews));
        }
    }
}
