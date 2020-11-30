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
    public class UserManagementController : Controller
    {
        public readonly IUserManagementServiceFacade _userManagementService;

        public UserManagementController(IUserManagementServiceFacade userManagementService)
        {
            _userManagementService = userManagementService ?? throw new ArgumentNullException(nameof(userManagementService));
        }

        /// <summary>
        /// Returns the user management section
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("user-management")]
        public async Task<IActionResult> Index(SimplePaginationViewModel viewModel)
        {
            var users = await _userManagementService.GetAllUserAsync(viewModel).ConfigureAwait(false);
            return View(users);
        }

        /// <summary>
        /// Returns the role management section
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("user-management/roles")]
        public async Task<IActionResult> GetRole()
        {
            var roles = await _userManagementService.GetAllRolesAsync().ConfigureAwait(false);
            return View("RoleView", roles);
        }

        /// <summary>
        /// Returns the admin management section
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("user-management/admins")]
        public IActionResult GetAdmin()
        {
            return View("AdminView");
        }
    }
}
