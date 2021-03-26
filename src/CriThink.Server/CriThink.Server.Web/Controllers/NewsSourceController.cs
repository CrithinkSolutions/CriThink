using System;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using CriThink.Common.Endpoints.DTOs.UnknownNewsSource;
using CriThink.Server.Core.Interfaces;
using CriThink.Server.Web.ActionFilters;
using CriThink.Server.Web.Models.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable CA1062 // Validate arguments of public methods

namespace CriThink.Server.Web.Controllers
{
    /// <summary>
    /// This controller contains API to manage black and whitelist
    /// </summary>
    [ApiVersion(EndpointConstants.VersionOne)]
    [ApiValidationFilter]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route(EndpointConstants.ApiBase + EndpointConstants.NewsSourceBase)]
    public class NewsSourceController : Controller
    {
        private readonly INewsSourceService _newsSourceService;
        private readonly IUnknownNewsSourceService _unknownNewsSourceService;

        public NewsSourceController(INewsSourceService newsSourceService, IUnknownNewsSourceService unknownNewsSourceService)
        {
            _newsSourceService = newsSourceService ?? throw new ArgumentNullException(nameof(newsSourceService));
            _unknownNewsSourceService = unknownNewsSourceService ?? throw new ArgumentNullException(nameof(unknownNewsSourceService));
        }

        /// <summary>
        /// Search the given source and returns the list that contains it
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET: /api/news-source/search?{newsLink}
        /// 
        /// </remarks>
        /// <param name="request">Source to search</param>
        /// <response code="200">Returns the classification, the description and related
        /// debunking news</response>
        /// <response code="400">If the request body is invalid</response>
        /// <response code="401">If the user is not authorized</response>
        /// <response code="404">If the given domain is not found</response>
        /// <response code="500">If the server can't process the request</response>
        /// <response code="503">If the server is not ready to handle the request</response>
        [ProducesResponseType(typeof(NewsSourceSearchWithDebunkingNewsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiBadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status503ServiceUnavailable)]
        [Produces("application/json")]
        [Route(EndpointConstants.NewsSourceSearch)]
        [HttpGet]
        public async Task<IActionResult> SearchNewsSourceAsync([FromQuery] NewsSourceSearchRequest request)
        {
            var searchResponse = await _newsSourceService.SearchNewsSourceWithAlertAsync(request.NewsLink).ConfigureAwait(false);
            return Ok(new ApiOkResponse(searchResponse));
        }

        /// <summary>
        /// Register the user for being notified when a news source is analyzed
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST: /api/news-source/register-for-notification
        ///     {
        ///         "email": "email",
        ///         "uri": "uri",
        ///     }
        ///         
        /// </remarks>
        /// <param name="request">Email and the unknown uri</param>
        /// <response code="204">Returns when operation succeeds</response>
        /// <response code="400">If the request body is invalid</response>
        /// <response code="401">If the user is not authorized</response>
        /// <response code="404">If the given domain is not found</response>
        /// <response code="500">If the server can't process the request</response>
        /// <response code="503">If the server is not ready to handle the request</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiBadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status503ServiceUnavailable)]
        [Produces("application/json")]
        [Route(EndpointConstants.NewsSourceRegisterForNotification)]
        [HttpPost]
        public async Task<IActionResult> RequestNotificationForUnknownSourceAsync([FromBody] NewsSourceNotificationForUnknownDomainRequest request)
        {
            await _unknownNewsSourceService.RequestNotificationForUnknownNewsSourceAsync(request).ConfigureAwait(false);
            return NoContent();
        }
    }
}
