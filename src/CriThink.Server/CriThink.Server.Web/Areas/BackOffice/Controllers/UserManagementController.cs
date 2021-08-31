using System;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Server.Application.Commands;
using CriThink.Server.Application.Queries;
using CriThink.Server.Core.Exceptions;
using CriThink.Server.Web.Areas.BackOffice.ViewModels;
using CriThink.Server.Web.Areas.BackOffice.ViewModels.UserManagement;
using CriThink.Server.Web.Models.DTOs;
using MediatR;
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
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route(EndpointConstants.UserManagementBase)]
    public class UserManagementController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IIdentityQueries _identityQueries;

        public UserManagementController(IMediator mediator, IIdentityQueries identityQueries)
        {
            _mediator = mediator ??
                throw new ArgumentNullException(nameof(mediator));

            _identityQueries = identityQueries ??
                throw new ArgumentNullException(nameof(identityQueries));
        }

        /// <summary>
        /// Returns the user management section
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index(SimplePaginationViewModel viewModel)
        {
            var users = await _identityQueries.GetAllUsersAsync(
                viewModel.PageSize,
                viewModel.PageIndex);

            return View(users);
        }

        /// <summary>
        /// Returns the role management section
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(EndpointConstants.UserManagementRoles)]
        public async Task<IActionResult> GetRole()
        {
            var roles = await _identityQueries.GetAllRolesAsync();
            return View("RoleView", roles);
        }

        /// <summary>
        /// Returns the add user view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(EndpointConstants.UserManagementAddUser)]
        public IActionResult AddUserView()
        {
            return View("AddUserView");
        }

        /// <summary>
        /// Make a new user
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route(EndpointConstants.UserManagementAddUser)]
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
                var command = new CreateUserCommand(
                    viewModel.UserName,
                    viewModel.Email,
                    viewModel.Password,
                    null);

                await _mediator.Send(command);

                viewModel.Message = "User Added!";
                return View("AddUserView", viewModel);
            }
            catch (ResourceNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Returns the add admin view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(EndpointConstants.UserManagementAddAdmin)]
        public IActionResult AddAdminView()
        {
            return View("AddAdminView");
        }

        /// <summary>
        /// Make a new admin user
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route(EndpointConstants.UserManagementAddAdmin)]
        public async Task<IActionResult> AddAdminAsync(AddUserViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            if (!ModelState.IsValid)
            {
                return View("AddUserView", viewModel);
            }

            try
            {
                var command = new CreateNewUserAsAdminCommand(
                    viewModel.Email,
                    viewModel.UserName,
                    viewModel.Password);

                await _mediator.Send(command);

                viewModel.Message = "Admin Added!";
                return View("AddUserView", viewModel);
            }
            catch (ResourceNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Softremove a user
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route(EndpointConstants.UserManagementSoftRemoveUser)]
        public async Task<IActionResult> SoftDeleteUserAsync(SimpleUserManagementViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            try
            {
                if (ModelState.IsValid)
                {
                    var command = new DeleteUserCommand(
                        viewModel.Id);

                    await _mediator.Send(command);
                }

                return RedirectToAction("Index");
            }
            catch (ResourceNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Get info by id for a user
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route(EndpointConstants.UserManagementInfoUser)]
        public async Task<IActionResult> GetUserByIdAsync(SimpleUserManagementViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            try
            {
                var response = await _identityQueries.GetUserByIdAsync(viewModel.Id);
                return Ok(new ApiOkResponse(response));
            }
            catch (Exception ex)
            {
                return (IActionResult) ex;
            }
        }

        /// <summary>
        /// Update property a user
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route(EndpointConstants.UserManagementEditUser)]
        public async Task<IActionResult> UpdateUserAsync(UserUpdateViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            try
            {
                if (ModelState.IsValid)
                {
                    var command = new UpdateUserCommand(
                        viewModel.Id,
                        viewModel.UserName,
                        viewModel.IsEmailConfirmed,
                        viewModel.IsLockoutEnabled,
                        viewModel.LockoutEnd);

                    await _mediator.Send(command);
                }

                return RedirectToAction("Index");
            }
            catch (ResourceNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Update role for a user
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route(EndpointConstants.UserManagementEditRoleUser)]
        public async Task<IActionResult> UpdateUserRoleAsync(UserRoleUpdateViewModel viewModel)
        {
            if (viewModel is null)
                throw new ArgumentNullException(nameof(viewModel));

            try
            {
                if (ModelState.IsValid)
                {
                    var command = new UpdateUserRoleCommand(
                        viewModel.Id,
                        viewModel.Role);

                    await _mediator.Send(command);
                }

                return RedirectToAction("Index");
            }
            catch (ResourceNotFoundException)
            {
                return NotFound();
            }
        }

    }
}
