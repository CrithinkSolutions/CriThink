using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CriThink.Server.Web.Areas.BackOffice.Controllers
{
    /// <summary>
    /// Controller to handle the backoffice debunking news operations
    /// </summary>
    [Authorize]
    [Area("BackOffice")]
    public class DebunkingNewsController : Controller
    {
        /// <summary>
        /// Returns the debunking news section
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
