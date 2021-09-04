using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CriThink.Server.Web.Areas.BackOffice.Controllers
{
    /// <summary>
    /// Controller to handle the backoffice home operations
    /// </summary>
    [AllowAnonymous]
    [Area("BackOffice")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : Controller
    {
        /// <summary>
        /// Returns the control panel page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("error/{code}")]
        public IActionResult Index(int code)
        {
            ViewData["ErrorMessage"] = $"{code}";
            return View();
        }
    }
}