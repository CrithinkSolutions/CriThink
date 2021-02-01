using System;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Server.Core.Exceptions;
using CriThink.Server.Web.Areas.BackOffice.ViewModels;
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
        public async Task<IActionResult> Index(SimplePaginationViewModel viewModel)
        {
            var news = await _newsSourceFacade.GetAllNewsSourcesAsync(viewModel).ConfigureAwait(false);
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
                viewModel.Message = "Source " + newsSource.Uri +" successfully added";
                return View(viewModel);
            }
            catch (ResourceNotFoundException)
            {
                return NotFound();
            }

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
        [Route(EndpointConstants.Edit)] // news-source/edit
        [HttpGet]
        public async Task<IActionResult> Edit(string newsSourceLink)
        {
            if (string.IsNullOrEmpty(newsSourceLink))
                return RedirectToAction(nameof(Index));

            if (!Uri.TryCreate($"https://{newsSourceLink}", UriKind.Absolute, out Uri uri))
                return RedirectToAction(nameof(Index));

            var searchResult = await _newsSourceFacade.SearchNewsSourceAsync(uri).ConfigureAwait(false);

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
        [Route(EndpointConstants.Edit)] // news-source/edit
        [HttpPost]
        public async Task<IActionResult> Edit(EditNewsSourceViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            if (!Uri.TryCreate(viewModel.OldLink, UriKind.Absolute, out Uri oldLink))
                return RedirectToAction(nameof(Index));

            if (!Uri.TryCreate(viewModel.NewLink, UriKind.Absolute, out Uri newLink))
                return RedirectToAction(nameof(Index));

            var searchResult = await _newsSourceFacade.SearchNewsSourceAsync(oldLink).ConfigureAwait(false);

            if (searchResult is null)
                return RedirectToAction(nameof(Index));

            if (searchResult.Classification == Classification.Conspiracist || searchResult.Classification == Classification.FakeNews)
            {
                await _newsSourceFacade.RemoveBlacklistNewsSourceAsync(oldLink).ConfigureAwait(false);
            }
            else
            {
                await _newsSourceFacade.RemoveWhitelistNewsSourceAsync(oldLink).ConfigureAwait(false);
            }

            var newsSource = new NewsSourceViewModel
            {
                Classification = viewModel.Classification,
                Uri = viewModel.NewLink,
            };

            await _newsSourceFacade.AddNewsSourceAsync(newsSource).ConfigureAwait(false);

            return RedirectToAction(nameof(Index));
        }
    }
}