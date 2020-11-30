using System;
using System.Threading.Tasks;
using CriThink.Server.Web.Areas.BackOffice.ViewModels;
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
    [Area("BackOffice")]
    public class NewsSourceController : Controller
    {
        private readonly INewsSourceServiceFacade _newsSourceServiceFacade;

        public NewsSourceController(INewsSourceServiceFacade newsSourceService)
        {
            _newsSourceServiceFacade = newsSourceService ?? throw new ArgumentNullException(nameof(newsSourceService));
        }

        /// <summary>
        /// Returns the news source section
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("news-source")]
        public async Task<IActionResult> Index()
        {
            var news = await _newsSourceServiceFacade.GetAllNewsSourcesAsync().ConfigureAwait(false);
            return View(news);
        }
    }
}