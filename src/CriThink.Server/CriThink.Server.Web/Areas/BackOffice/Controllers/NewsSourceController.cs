using System;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Server.Web.Areas.BackOffice.ViewModels.NewsSource;
using CriThink.Server.Web.Facades;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable CA1062 // Validate arguments of public methods

namespace CriThink.Server.Web.Areas.BackOffice.Controllers
{
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

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var news = await _newsSourceFacade.GetAllNewsSourcesAsync().ConfigureAwait(false);
            return View(news);
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route(EndpointConstants.Add)]  // news-source/add
        [HttpGet]
        public IActionResult AddSource()
        {
            return View();
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route(EndpointConstants.Add)]  // news-source/add
        [HttpPost]
        public async Task<IActionResult> AddSource(AddNewsSourceViewModel viewModel)
        {
            await _newsSourceFacade.AddNewsSourceAsync(viewModel).ConfigureAwait(false);
            return NoContent();
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route(EndpointConstants.NewsSourceRemoveWhiteNewsSource)] // news-source/whitelist
        [HttpDelete]
        public async Task<IActionResult> RemoveGoodNewsSourceAsync(RemoveWhitelistViewModel viewModel)
        {
            var uri = new Uri(viewModel.Uri);
            await _newsSourceFacade.RemoveWhitelistNewsSourceAsync(uri).ConfigureAwait(false);
            return NoContent();
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route(EndpointConstants.NewsSourceRemoveBlackNewsSource)] // news-source/blacklist
        [HttpDelete]
        public async Task<IActionResult> RemoveBadNewsSourceAsync(RemoveBlacklistViewModel viewModel)
        {
            var uri = new Uri(viewModel.Uri);
            await _newsSourceFacade.RemoveBlacklistNewsSourceAsync(uri).ConfigureAwait(false);
            return NoContent();
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route(EndpointConstants.NewsSourceTriggerIdentifiedSource)] // news-source/identify
        [HttpGet]
        public IActionResult TriggerIdentifiedNewsSourceAsync(Guid unknownNewsId)
        {
            return View(unknownNewsId);
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route(EndpointConstants.NewsSourceTriggerIdentifiedSource)] // news-source/identify
        [HttpPost]
        public async Task<IActionResult> TriggerIdentifiedNewsSourceAsync()
        {
            return NoContent();
        }
    }
}