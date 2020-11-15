using System;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Server.Core.Interfaces;
using CriThink.Server.Web.ActionFilters;
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
    [Route(EndpointConstants.ApiBase + EndpointConstants.DebunkNewsBase)] //api/debunking-news
    public class DebunkingNewsController : Controller
    {
        private readonly IDebunkingNewsService _debunkNewsService;

        public DebunkingNewsController(IDebunkingNewsService debunkNewsService)
        {
            _debunkNewsService = debunkNewsService ?? throw new ArgumentNullException(nameof(debunkNewsService));
        }

        /// <summary>
        /// Trigger the update of internal debunking news repository
        /// </summary>
        /// <returns>HTTP status code</returns>
        [AllowAnonymous]
        [Route(EndpointConstants.DebunkNewsTriggerUpdate)] // api/debunking-news/trigger
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> TriggerRepositoryUpdateAsync()
        {
            await _debunkNewsService.UpdateRepositoryAsync().ConfigureAwait(false);
            return NoContent();
        }
    }
}
