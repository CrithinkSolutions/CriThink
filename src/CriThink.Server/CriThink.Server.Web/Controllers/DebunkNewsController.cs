using System;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Server.Web.ActionFilters;
using CriThink.Server.Web.Services;
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
    [Route(EndpointConstants.ApiBase + EndpointConstants.DebunkNewsBase)] //api/debunk-news
    public class DebunkNewsController : Controller
    {
        private readonly IDebunkNewsService _debunkNewsService;

        public DebunkNewsController(IDebunkNewsService debunkNewsService)
        {
            _debunkNewsService = debunkNewsService ?? throw new ArgumentNullException(nameof(debunkNewsService));
        }

        /// <summary>
        /// Trigger the update of internal debunked neews repository
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route(EndpointConstants.DebunkNewsTriggerUpdate)] // api/debunk-news/trigger
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
