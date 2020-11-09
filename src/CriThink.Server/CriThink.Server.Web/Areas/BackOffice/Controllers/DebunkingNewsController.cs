using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CriThink.Server.Web.Services;
using CriThink.Common.Endpoints.DTOs.Admin;
using System;

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
            var request = new DebunkingNewsGetAllRequest
            {
                PageSize = 30,
                PageIndex = 1
            };

            var allnews = await _debunkingNewsService.GetAllDebunkingNewsAsync(request).ConfigureAwait(false);
            return View(allnews);
        }
    }
}
