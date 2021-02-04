using System;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Server.Core.Exceptions;
using CriThink.Server.Web.Areas.BackOffice.ViewModels;
using CriThink.Server.Web.Areas.BackOffice.ViewModels.DebunkingNews;
using CriThink.Server.Web.Facades;
using CriThink.Server.Web.Models.DTOs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CriThink.Server.Web.Areas.BackOffice.Controllers
{
    /// <summary>
    /// Controller to handle the backoffice debunking news operations
    /// </summary>
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Admin")]
    [Area("BackOffice")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route(EndpointConstants.DebunkNewsBase)]
    public class DebunkingNewsController : Controller
    {
        private readonly IDebunkingNewsServiceFacade _debunkingNewsServiceFacade;

        public DebunkingNewsController(IDebunkingNewsServiceFacade debunkingNewsServiceFacade)
        {
            _debunkingNewsServiceFacade = debunkingNewsServiceFacade ?? throw new ArgumentNullException(nameof(debunkingNewsServiceFacade));
        }

        /// <summary>
        /// Returns the debunking news section
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index(SimplePaginationViewModel viewModel)
        {
            var news = await _debunkingNewsServiceFacade.GetAllDebunkingNewsAsync(viewModel).ConfigureAwait(false);
            return View(news);
        }

        /// <summary>
        /// Returns the add debunking news page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(EndpointConstants.DebunkingNewsAddNews)]
        public ActionResult AddNewsView()
        {
            return View("AddNewsView", new AddNewsViewModel());
        }

        /// <summary>
        /// Add debunking news
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route(EndpointConstants.DebunkingNewsAddNews)]
        public async Task<IActionResult> AddNewsAsync(AddNewsViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            if (!ModelState.IsValid)
            {
                return View("AddNewsView", viewModel);
            }

            try
            {
                await _debunkingNewsServiceFacade.AddDebunkingNewsAsync(viewModel).ConfigureAwait(false);
                viewModel.Message = "News Added!";
                return View("AddNewsView", viewModel);
            }
            catch (ResourceNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Remove debunking news
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route(EndpointConstants.DebunkingNewsRemoveNews)]
        public async Task<IActionResult> RemoveNewsAsync(SimpleDebunkingNewsViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            try
            {
                if (ModelState.IsValid)
                {
                    await _debunkingNewsServiceFacade.DeleteDebunkingNewsAsync(viewModel).ConfigureAwait(false);
                }

                return RedirectToAction("Index");
            }
            catch (ResourceNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Info debunking news by id
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route(EndpointConstants.DebunkingNewsInfoNews)]
        public async Task<IActionResult> GetDebunkingNewsAsync(SimpleDebunkingNewsViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            try
            {
                var info = await _debunkingNewsServiceFacade.GetDebunkingNewsAsync(viewModel).ConfigureAwait(false);
                return Ok(new ApiOkResponse(info));

            }
            catch (ResourceNotFoundException)
            {
                return NotFound();
            }
        }

        [Route(EndpointConstants.Edit)]
        [HttpGet]
        public async Task<IActionResult> EditDebunkingNewsAsync(Guid id)
        {
            var debunkingNews = await _debunkingNewsServiceFacade.GetDebunkingNewsAsync(new SimpleDebunkingNewsViewModel
            {
                Id = id,
            }).ConfigureAwait(false);

            if (debunkingNews is not null)
            {
                var viewModel = new EditDebunkingNewsViewModel
                {
                    Title = debunkingNews.Title,
                    Caption = debunkingNews.Caption,
                    Link = debunkingNews.Link,
                    ImageLink = debunkingNews.ImageLink,
                    Keywords = string.Join(", ", debunkingNews.Keywords),
                };

                return View(viewModel);
            }

            else
                return NotFound();
        }

        [Route(EndpointConstants.Edit)]
        [HttpPost]
        public async Task<IActionResult> EditDebunkingNewsAsync(EditDebunkingNewsViewModel viewModel)
        {
            if (viewModel is null)
                throw new ArgumentNullException(nameof(viewModel));

            if (!ModelState.IsValid)
                return View(viewModel);

            await _debunkingNewsServiceFacade.UpdateDebunkingNewsAsync(viewModel).ConfigureAwait(false);

            return RedirectToAction(nameof(Index));
        }
    }
}
