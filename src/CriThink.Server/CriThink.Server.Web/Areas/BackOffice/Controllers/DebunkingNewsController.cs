using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CriThink.Server.Web.Services;
using CriThink.Common.Endpoints.DTOs.Admin;
using System;
using CriThink.Server.Web.Exceptions;
using System.Collections.Generic;

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
        public async Task<IActionResult> Index(int pageSize, int pageIndex)
        {
            var news = await GetAllNews(pageSize,pageIndex).ConfigureAwait(false);
            return View(news);
        }
        /// <summary>
        /// Returns the add debunking news page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddNewsView()
        {
            return View("AddNews");
        }

        /// <summary>
        /// Returns the remove debunking news page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> RemoveNewsView(bool success, int pageSize, int pageIndex)
        {
            if (success)
            {
                TempData["success"] = "News removed!"; 
            }

            var news = await GetAllNews(pageSize,pageIndex).ConfigureAwait(false);
            return View("RemoveNews", news);
        }

        /// <summary>
        /// Returns all debunking news
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IList<DebunkingNewsGetAllResponse>> GetAllNews(int pageSize, int pageIndex) 
        {
            if (pageSize == 0 || pageIndex == 0 ) 
            {
                pageSize = 20;
                pageIndex = 1;
            }

            var request = new DebunkingNewsGetAllRequest
            {
                PageSize = pageSize,
                PageIndex = pageIndex
            };

            var allnews = await _debunkingNewsService.GetAllDebunkingNewsAsync(request).ConfigureAwait(false);
            return allnews;
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
