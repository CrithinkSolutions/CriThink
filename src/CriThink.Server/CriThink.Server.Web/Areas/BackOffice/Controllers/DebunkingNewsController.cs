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
        public async Task<IActionResult> Index()
        {
            var news = await GetAllNews(30,1).ConfigureAwait(false);
            return View(news);
        }

        [HttpGet]
        public ActionResult AddNewsView()
        {
            return View("AddNews");
        }

        [HttpGet]
        public async Task<IActionResult> RemoveNewsView()
        {
            var news = await GetAllNews(30,1).ConfigureAwait(false);
            return View("RemoveNews", news);
        }

        [HttpGet]
        public async Task<IList<DebunkingNewsGetAllResponse>> GetAllNews(int pageSize, int pageIndex) 
        {
            var request = new DebunkingNewsGetAllRequest
            {
                PageSize = pageSize,
                PageIndex = pageIndex
            };

            var allnews = await _debunkingNewsService.GetAllDebunkingNewsAsync(request).ConfigureAwait(false);
            return allnews;

        }

        [HttpPost]
        public async Task<IActionResult> AddNews(DebunkingNewsAddRequest addnewsModel)
        {
            if(addnewsModel == null)
                throw new ArgumentNullException(nameof(addnewsModel));
            
            try 
            {
                await _debunkingNewsService.AddDebunkingNewsAsync(addnewsModel).ConfigureAwait(false);
                return Ok();
            }
            catch (ResourceNotFoundException) 
            {
               return BadRequest();
            }
        }
    }
}
