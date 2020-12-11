using System;
using System.Threading.Tasks;
using CriThink.Server.Core.Exceptions;
using CriThink.Server.Web.Areas.BackOffice.ViewModels;
using CriThink.Server.Web.Areas.BackOffice.ViewModels.DebunkingNews;
using CriThink.Server.Web.Facades;
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
    public class DebunkingNewsController : Controller
    {
        private readonly IDebunkingNewsServiceFacade _debunkingNewsServiceFacade;

        public DebunkingNewsController(IDebunkingNewsServiceFacade debunkingNewsService)
        {
            _debunkingNewsServiceFacade = debunkingNewsService ?? throw new ArgumentNullException(nameof(debunkingNewsService));
        }

        /// <summary>
        /// Returns the debunking news section
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("debunking-news")]
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
        [Route("add-news")]
        public ActionResult AddNewsView()
        {
            return View("AddNewsView", new AddNewsViewModel());
        }

        /// <summary>
        /// Returns the remove debunking news page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("remove-news")]
        public async Task<IActionResult> RemoveNewsViewAsync(SimplePaginationViewModel viewModel)
        {
            var news = await _debunkingNewsServiceFacade.GetAllDebunkingNewsAsync(viewModel).ConfigureAwait(false);
            return View("RemoveNewsView", news);
        }

        /// <summary>
        /// Add debunking news
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("add-news")]
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
        [Route("remove-news", Name = "RemoveNewsAsync")]
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
    }
}
