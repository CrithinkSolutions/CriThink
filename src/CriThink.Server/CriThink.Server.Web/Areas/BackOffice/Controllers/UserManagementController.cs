using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CriThink.Server.Web.Areas.BackOffice.Controllers
{
    /// <summary>
    /// Controller to handle the backoffice operations
    /// </summary>
    [Authorize]
    [Area("BackOffice")]
    public class UserManagementController : Controller
    {
        /// <summary>
        /// Returns the user management section
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
