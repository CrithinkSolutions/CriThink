using System;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Common.Endpoints.DTOs.Common;
using CriThink.Common.Endpoints.DTOs.UnknownNewsSource;
using CriThink.Server.Core.Interfaces;
using CriThink.Server.Web.ActionFilters;
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
    [Route(EndpointConstants.ApiBase + EndpointConstants.NewsSourceBase)] //api/news-source
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
        /// <param name="request">Source to search</param>
        /// <returns>Returns the list where the source is contained</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [Route(EndpointConstants.NewsSourceSearch)] // api/news-source/search
        [HttpGet]
        public async Task<IActionResult> SearchNewsSourceAsync([FromQuery] SimpleUriRequest request)
        {
            var uri = new Uri(request.Uri);
            var searchResponse = await _newsSourceService.SearchNewsSourceWithAlertAsync(uri).ConfigureAwait(false);

            if (searchResponse is null)
                return NotFound();

            return Ok(searchResponse);
        }

        /// <summary>
        /// Register the user for being notified if a news source is analyzed
        /// </summary>
        /// <param name="request">User info</param>
        /// <returns>Returns the list where the source is contained</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [Route(EndpointConstants.NewsSourceRegisterForNotification)] // api/news-source/register-for-notification
        [HttpPost]
        public async Task<IActionResult> RequestNotificationForUnknownSourceAsync([FromBody] NewsSourceNotificationForUnknownDomainRequest request)
        {
            await _unknownNewsSourceService.RequestNotificationForUnknownNewsSourceAsync(request).ConfigureAwait(false);

            return NoContent();
        }
    }
}
