using Microsoft.AspNetCore.Mvc;

namespace CriThink.Server.Web.Controllers
{
    public class BackOfficeController : Controller
    {
        /// <summary>
        /// Returns the home page
        /// </summary>
        /// <returns></returns>
        [Route("/")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Returns the control panel page
        /// </summary>
        /// <returns></returns>
        [Route("/control-panel")]
        [HttpGet]
        public IActionResult Control()
        {
            return View();
        }

        /// <summary>
        /// Returns the debunking news section
        /// </summary>
        /// <returns></returns>
        [Route("/debunking-news")]
        [HttpGet]
        public IActionResult DebunkingNews()
        {
            return View();
        }

        /// <summary>
        /// Returns the user management section
        /// </summary>
        /// <returns></returns>
        [Route("/user-management")]
        [HttpGet]
        public IActionResult UserManagement()
        {
            return View();
        }
    }
}

