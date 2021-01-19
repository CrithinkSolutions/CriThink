using System;
using System.Threading.Tasks;
using CriThink.Server.Core.Exceptions;
using CriThink.Server.Web.Areas.BackOffice.ViewModels;
using CriThink.Server.Web.Areas.BackOffice.ViewModels.UserManagement;
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
        public readonly IUserManagementServiceFacade _userManagementServiceFacade;

        public UserManagementController(IUserManagementServiceFacade userManagementServiceFacade)
        {
            _userManagementServiceFacade = userManagementServiceFacade ?? throw new ArgumentNullException(nameof(userManagementServiceFacade));
        }

        /// <summary>
        /// Returns the user management section
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("user-management")]
        public async Task<IActionResult> Index(SimplePaginationViewModel viewModel)
        {
            var users = await _userManagementServiceFacade.GetAllUserAsync(viewModel).ConfigureAwait(false);
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
            var roles = await _userManagementServiceFacade.GetAllRolesAsync().ConfigureAwait(false);
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

        /// <summary>
        /// Returns the user management section
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("user-management/add-user")]
        public IActionResult AddUserView()
        {
            return View("AddUserView", new AddUserViewModel());
        }

        /// <summary>
        /// Returns the user management section
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("user-management/add-user")]
        public async Task<IActionResult> AddUserAsync(AddUserViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            if (!ModelState.IsValid)
            {
                return View("AddUserView", viewModel);
            }

            try
            {
                await _userManagementServiceFacade.CreateNewUserAsync(viewModel).ConfigureAwait(false);
                viewModel.Message = "User Added!";
                return View("AddUserView", viewModel);
            }
            catch(ResourceNotFoundException)
            {
               return NotFound();
            }
        }
    }
}
