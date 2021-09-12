using System;
using System.Security.Claims;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Common.Helpers;
using CriThink.Server.Application.Commands;
using CriThink.Server.Web.Areas.BackOffice.ViewModels.Account;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Web.Areas.BackOffice.Controllers
{
    /// <summary>
    /// Controller to handle the backoffice account operations
    /// </summary>
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Admin")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Area("BackOffice")]
    public class AccountController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            IMediator mediator,
            ILogger<AccountController> logger)
        {
            _mediator = mediator ??
                throw new ArgumentNullException(nameof(mediator));

            _logger = logger;
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
            if (HttpContext.User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            var model = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ImagePath = GetRandomImageForLoginView(),
            };

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
            if (!ModelState.IsValid)
            {
                viewModel.ImagePath = GetRandomImageForLoginView();
                return View(nameof(Index), viewModel);
            }

            try
            {
                var (email, username) = GetEmailAndUsername(viewModel.EmailOrUsername);

                var command = new LoginCookieUserCommand(
                    email, username, viewModel.Password, viewModel.RememberMe);

                var claimsIdentity = await _mediator.Send(command);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return Redirect(viewModel.ReturnUrl ?? "/");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error performing login on backoffice");
                viewModel.ImagePath = GetRandomImageForLoginView();
                return View(nameof(Index), viewModel);
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
        public IActionResult ForgotPassword()
        {
            if (HttpContext.User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            return View();
        }

        /// <summary>
        /// Send a email to reset password
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route(EndpointConstants.MvcForgotPassword)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(ForgotPassword), viewModel);
            }

            viewModel.Message = "We sent you an email if the user exists";

            var (email, username) = GetEmailAndUsername(viewModel.EmailOrUsername);

            try
            {
                var command = new ForgotPasswordCommand(
                    email, username);

                await _mediator.Send(command);

                viewModel.Message = "Email sent!";
                return View(viewModel);
            }
            catch (Exception)
            {
                _logger?.LogWarning("Error resetting password from the backoffice");
                ModelState.AddModelError(string.Empty, "An error occurred. Please verify your credentials");
            }
            return View(nameof(ForgotPassword), viewModel);

        }

        private static string GetRandomImageForLoginView()
        {
            var randomImageIndex = new Random().Next(1, 6);
            return $"/images/login_intro_{randomImageIndex}.svg";
        }

        private (string email, string username) GetEmailAndUsername(string value)
        {
            string email = null, username = null;

            if (EmailHelper.IsEmail(value))
            {
                email = value;
            }
            else
            {
                username = value;
            }

            return (email, username);
        }
    }
}
