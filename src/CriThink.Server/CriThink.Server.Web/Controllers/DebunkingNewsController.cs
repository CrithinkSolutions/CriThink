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
    [Route(EndpointConstants.ApiBase + EndpointConstants.DebunkNewsBase)] //api/debunking-news
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
        /// <returns>HTTP status code</returns>
        [AllowAnonymous]
        [ServiceFilter(typeof(CrossServiceAuthenticationFilter))]
        [Route(EndpointConstants.DebunkNewsTriggerUpdate)] // api/debunking-news/trigger
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        /// <param name="request">Page index and Debunking news per page</param>
        /// <returns></returns>
        [Route(EndpointConstants.DebunkingNewsGetAll)] // api/debunking-news/all
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
        /// <param name="request">Debunking news guid</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [HttpGet] //api/debunking-news/
        public async Task<IActionResult> GetDebunkingNewsAsync([FromQuery] DebunkingNewsGetRequest request)
        {
            var debunkingNews = await _debunkingNewsService.GetDebunkingNewsAsync(request).ConfigureAwait(false);
            return Ok(new ApiOkResponse(debunkingNews));
        }
    }
}
