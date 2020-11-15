using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CriThink.Server.Web.Services;
using CriThink.Common.Endpoints.DTOs.Admin;
using System;
using CriThink.Server.Web.Areas.BackOffice.ViewModels.Shared;
using CriThink.Server.Web.Exceptions;

namespace CriThink.Server.Web.Areas.BackOffice.Controllers
{
    /// <summary>
    /// Controller to handle the backoffice debunking news operations
    /// </summary>
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Admin")]
    [Area("BackOffice")]
    public class DebunkingNewsController : Controller
    {
        private readonly IDebunkNewsService _debunkingNewsService;

        public DebunkingNewsController(IDebunkNewsService debunkingNewsService) 
        {
            _debunkingNewsService = debunkingNewsService ?? throw new ArgumentNullException(nameof(debunkingNewsService));
        }

        /// <summary>
        /// Returns the debunking news section
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("debunking-news")]
        public async Task<IActionResult> Index(SimplePagification pagification)
        {
            var news = await _debunkingNewsService.GetAllDebunkingNewsAsync(pagification.pageSize, pagification.pageIndex).ConfigureAwait(false);
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
            return View("AddNews");
        }

        /// <summary>
        /// Returns the remove debunking news page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("remove-news")]
        public async Task<IActionResult> RemoveNewsView(SimplePagification pagification)
        {
            var news =  await _debunkingNewsService.GetAllDebunkingNewsAsync(pagification.pageSize, pagification.pageIndex).ConfigureAwait(false);
            return View("RemoveNews", news);
        }

        /// <summary>
        /// Add debunking news
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddNews(DebunkingNewsAddRequest addnewsModel)
        {
            if(addnewsModel == null)
                throw new ArgumentNullException(nameof(addnewsModel));

            if (!ModelState.IsValid)  
            {  
                return View("AddNews", addnewsModel);
            }

            try 
            {
                await _debunkingNewsService.AddDebunkingNewsAsync(addnewsModel).ConfigureAwait(false);
                TempData["success"] = "News added!"; 
                return View("AddNews");
            }
            catch (ResourceNotFoundException) 
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Remove debunking news
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> RemoveNews(SimpleDebunkingNewsRequest removenewsModel)
        {
            if(removenewsModel == null)
                throw new ArgumentNullException(nameof(removenewsModel));

            try 
            {
                await _debunkingNewsService.DeleteDebunkingNewsAsync(removenewsModel).ConfigureAwait(false);
                return RedirectToAction("RemoveNewsView", new { success = true });
            }
            catch (ResourceNotFoundException) 
            {
                return BadRequest();
            }
        }
    }
}
