using System;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Server.Web.Areas.BackOffice.ViewModels;
using CriThink.Server.Web.Areas.BackOffice.ViewModels.NewsSource;
using CriThink.Server.Web.Facades;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable CA1062 // Validate arguments of public methods

namespace CriThink.Server.Web.Areas.BackOffice.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
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
        public async Task<IActionResult> Index(SimplePaginationViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.PageIndex = 0;
                viewModel.PageSize = 20;
            }

            var news = await _newsSourceFacade.GetAllNewsSourcesAsync(viewModel).ConfigureAwait(false);
            return View(news);
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route(EndpointConstants.MvcAdd)]  // news-source/add
        [HttpGet]
        public IActionResult AddSource()
        {
            return View();
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route(EndpointConstants.MvcAdd)]  // news-source/add
        [HttpPost]
        public async Task<IActionResult> AddSource(AddNewsSourceViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var newsSource = new NewsSourceViewModel
            {
                Classification = viewModel.Classification,
                Uri = viewModel.Uri,
            };

            try
            {
                await _newsSourceFacade.AddNewsSourceAsync(newsSource).ConfigureAwait(false);
                viewModel.Message = $"Source {newsSource.Uri} successfully added";
                return View(viewModel);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route(EndpointConstants.NewsSourceRemoveNewsSource)] // news-source/blacklist
        [HttpDelete]
        public async Task<IActionResult> RemoveNewsSourceAsync(RemoveBlacklistViewModel viewModel)
        {
            await _newsSourceFacade.RemoveNewsSourceAsync(viewModel.Uri).ConfigureAwait(false);
            return NoContent();
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route(EndpointConstants.MvcEdit)] // news-source/edit
        [HttpGet]
        public async Task<IActionResult> Edit(string newsSourceLink)
        {
            if (string.IsNullOrEmpty(newsSourceLink))
                return RedirectToAction(nameof(Index));

            var searchResult = await _newsSourceFacade.SearchNewsSourceAsync(newsSourceLink).ConfigureAwait(false);

            if (searchResult is null)
                return RedirectToAction(nameof(Index));

            var viewModel = new EditNewsSourceViewModel
            {
                OldLink = searchResult.Uri,

                NewLink = searchResult.Uri,
                Classification = searchResult.Classification,
            };

            return View(viewModel);
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route(EndpointConstants.MvcEdit)] // news-source/edit
        [HttpPost]
        public async Task<IActionResult> Edit(EditNewsSourceViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var searchResult = await _newsSourceFacade.SearchNewsSourceAsync(viewModel.OldLink).ConfigureAwait(false);

            if (searchResult is null)
                return RedirectToAction(nameof(Index));

            await _newsSourceFacade.RemoveNewsSourceAsync(viewModel.OldLink).ConfigureAwait(false);

            var newsSource = new NewsSourceViewModel
            {
                Classification = viewModel.Classification,
                Uri = viewModel.NewLink,
            };

            await _newsSourceFacade.AddNewsSourceAsync(newsSource).ConfigureAwait(false);

            return RedirectToAction(nameof(Index));
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route(EndpointConstants.NewsSourcesGetAllNotifications)] // news-source/notification-requests
        [HttpGet]
        public async Task<IActionResult> GetNotificationRequestsAsync(SimplePaginationViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.PageIndex = 0;
                viewModel.PageSize = 20;
            }

            var response = await _newsSourceFacade.GetPendingNotificationRequestsAsync(viewModel).ConfigureAwait(false);
            return View("NotificationRequests", response);
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route(EndpointConstants.NewsSourcesGetAllUnknownNewsSources)] // news-source/get-all-unknown
        [HttpGet]
        public async Task<IActionResult> GetUnknownNewsSourcesAsync(SimplePaginationViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.PageIndex = 0;
                viewModel.PageSize = 20;
            }

            var response = await _newsSourceFacade.GetUnknownNewsSourcesAsync(viewModel).ConfigureAwait(false);
            return View("Unknown", response);
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route(EndpointConstants.NewsSourceTriggerIdentifiedSource)] // news-source/identify
        [HttpGet]
        public async Task<IActionResult> Identify(Guid id)
        {
            var viewModel = await _newsSourceFacade.GetUnknownNewsSourceAsync(id).ConfigureAwait(false);

            return View(viewModel);
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route(EndpointConstants.NewsSourceTriggerIdentifiedSource)] // news-source/identify
        [HttpPost]
        public async Task<IActionResult> Identify(UnknownNewsSourceViewModel viewModel)
        {
            if (viewModel.Classification == Classification.Unknown)
                ModelState.AddModelError(nameof(UnknownNewsSourceViewModel.Classification), "You must identify the source");

            if (!ModelState.IsValid)
                return View(viewModel);

            await _newsSourceFacade.TriggerIdentifiedNewsSourceAsync(viewModel.Uri, viewModel.Classification).ConfigureAwait(false);

            return RedirectToAction(nameof(Index));
        }
    }
}