using System;
using System.Security.Claims;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Server.Core.Exceptions;
using CriThink.Server.Core.Interfaces;
using CriThink.Server.Web.Areas.BackOffice.ViewModels.Account;
using CriThink.Server.Web.Models.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable CA1054 // URI-like parameters should not be strings
namespace CriThink.Server.Web.Areas.BackOffice.Controllers
{
    /// <summary>
    /// Controller to handle the backoffice account operations
    /// </summary>
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Admin")]
    [Area("BackOffice")]
    public class AccountController : Controller
    {
        private readonly IIdentityService _identityService;
        public AccountController(IIdentityService identityService)
        {
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }

        /// <summary>
        /// Returns the backoffice home page
        /// </summary>
        /// <param name="returnUrl">Return uri</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index(string returnUrl)
        {
            var model = new LoginViewModel { ReturnUrl = returnUrl };
            return View(model);
        }

        /// <summary>
        /// Performs user login
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            try
            {
                var claimsIdentity = await _identityService.LoginUserAsync(viewModel.EmailOrUsername, viewModel.Password, viewModel.RememberMe)
                    .ConfigureAwait(false);

                await HttpContext
                    .SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity))
                    .ConfigureAwait(false);

                return Redirect(viewModel.ReturnUrl ?? "/");
            }
            catch (ResourceNotFoundException)
            {
                ModelState.AddModelError(nameof(viewModel.EmailOrUsername), "Login Failed: Invalid Email or password");
                return BadRequest();
            }
        }

        /// <summary>
        /// Perform user logout
        /// </summary>
        /// <returns>Redirects to home</returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).ConfigureAwait(false);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Returns the forgot password page
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route(EndpointConstants.MvcForgotPassword)]
        [HttpGet]
        public IActionResult ForgotPasswordAsync()
        {
            return View();
        }

        /// <summary>
        /// Send a email to reset password
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route(EndpointConstants.MvcForgotPassword)]
        [HttpPost]
        public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            try
            {
                if (viewModel.EmailOrUsername.Contains("@"))
                {
                    var email = viewModel.EmailOrUsername;
                    await _identityService.GenerateUserPasswordTokenAsync(email, null).ConfigureAwait(false);
                    viewModel.Message = "Email sent!";
                    return View(viewModel);
                }
                else
                {
                    var username = viewModel.EmailOrUsername;
                    await _identityService.GenerateUserPasswordTokenAsync(null, username).ConfigureAwait(false);
                    viewModel.Message = "Email sent!";
                    return View(viewModel);
                }
            }
            catch (Exception)
            {
                return View();
            }
        }
    }
}
