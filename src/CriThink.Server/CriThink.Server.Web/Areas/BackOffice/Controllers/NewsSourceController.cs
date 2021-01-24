using System;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Server.Web.Areas.BackOffice.ViewModels.NewsSource;
using CriThink.Server.Web.Facades;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CriThink.Server.Web.Areas.BackOffice.Controllers
{
    /// <summary>
    /// Controller to handle the backoffice operations
    /// </summary>
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Admin")]
    [Route(EndpointConstants.NewsSourceBase)]
    [Area("BackOffice")]
    public class NewsSourceController : Controller
    {
        private readonly INewsSourceFacade _newsSourceFacade;

        public NewsSourceController(INewsSourceFacade newsSourceFacade)
        {
            _newsSourceFacade = newsSourceFacade ?? throw new ArgumentNullException(nameof(newsSourceFacade));
        }

        /// <summary>
        /// Returns the news source section
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var news = await _newsSourceFacade.GetAllNewsSourcesAsync().ConfigureAwait(false);
            return View(news);
        }

        /// <summary>
        /// Add the given source to white or black list
        /// </summary>
        /// <param name="viewModel">Source to add</param>
        /// <returns>Returns the operation result</returns>
        [Authorize]
        [Produces("application/json")]
        [Route(EndpointConstants.Add)]
        [HttpPost]
        public async Task<IActionResult> AddSourceAsync(AddNewsSourceViewModel viewModel)
        {
            await _newsSourceFacade.AddNewsSourceAsync(viewModel).ConfigureAwait(false);
            return NoContent();
        }

        /// <summary>
        /// Remove the given source from the whitelist
        /// </summary>
        /// <param name="viewModel">Source to remove</param>
        /// <returns>Returns the operation result</returns>
        [Authorize]
        [Route(EndpointConstants.NewsSourceRemoveWhiteNewsSource)] // api/news-source/whitelist
        [HttpDelete]
        public async Task<IActionResult> RemoveGoodNewsSourceAsync(RemoveWhitelistViewModel viewModel)
        {
            var uri = new Uri(viewModel.Uri);
            await _newsSourceFacade.RemoveWhitelistNewsSourceAsync(uri).ConfigureAwait(false);
            return NoContent();
        }

        /// <summary>
        /// Remove the given source from the blacklist
        /// </summary>
        /// <param name="viewModel">Source to remove</param>
        /// <returns>Returns the operation result</returns>
        [Authorize]
        [Route(EndpointConstants.NewsSourceRemoveBlackNewsSource)] // api/news-source/blacklist
        [Produces("application/json")]
        [HttpDelete]
        public async Task<IActionResult> RemoveBadNewsSourceAsync(RemoveBlacklistViewModel viewModel)
        {
            var uri = new Uri(viewModel.Uri);
            await _newsSourceFacade.RemoveBlacklistNewsSourceAsync(uri).ConfigureAwait(false);
            return NoContent();
        }
    }
}