using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CriThink.Server.Web.Services;
using CriThink.Common.Endpoints.DTOs.Admin;
using System;
using CriThink.Server.Web.Areas.BackOffice.ViewModels.Shared;
using CriThink.Server.Web.Areas.BackOffice.ViewModels.DebunkingNews;
using CriThink.Server.Web.Areas.Public.ViewModel.Shared;
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
            return View("AddNews", new AddNewsViewModel());
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
        [Route("add-news")]
        public async Task<IActionResult> AddNews(AddNewsViewModel addnewsModel)
        {
            if(addnewsModel == null)
                throw new ArgumentNullException(nameof(addnewsModel));

            if (!ModelState.IsValid)  
            {  
                return View("AddNews", addnewsModel);
            }

            var addnews = new DebunkingNewsAddRequest 
            {
                Title = addnewsModel.Title,
                Caption = addnewsModel.Caption,
                Link = addnewsModel.Link,
                Keywords = addnewsModel.Keywords
            };

            try 
            {
                await _debunkingNewsService.AddDebunkingNewsAsync(addnews).ConfigureAwait(false);
                addnewsModel.Message = "News Added!";
                return View("AddNews", addnewsModel);
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
        [Route("remove-news")]
        public async Task<IActionResult> RemoveNews(RemoveNewsViewModel removenewsModel)
        {
            if(removenewsModel == null)
                throw new ArgumentNullException(nameof(removenewsModel));
            
            if (!ModelState.IsValid)  
            {  
                return View("RemoveNews", removenewsModel);
            }

            var removeNews = new SimpleDebunkingNewsRequest
            {
                Id = removenewsModel.Id
            };

            try 
            {
                await _debunkingNewsService.DeleteDebunkingNewsAsync(removeNews).ConfigureAwait(false);
                return RedirectToAction("RemoveNewsView");
            }
            catch (ResourceNotFoundException) 
            {
                return BadRequest();
            }
        }
    }
}
