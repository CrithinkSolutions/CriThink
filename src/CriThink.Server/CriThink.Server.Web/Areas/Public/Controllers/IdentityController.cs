using System;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Server.Core.Interfaces;
using CriThink.Server.Web.Areas.Publics.ViewModel;
using CriThink.Server.Web.Areas.Publics.ViewModel.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Web.Areas.Publics.Controllers
{
    [Area("Public")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class IdentityController : Controller
    {
        private readonly IIdentityService _identityService;
        private readonly ILogger<IdentityController> _logger;

        public IdentityController(IIdentityService identityService, ILogger<IdentityController> logger)
        {
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _logger = logger;
        }

        [AllowAnonymous]
        [Route(EndpointConstants.IdentityResetPassword)]
        [HttpGet]
        public IActionResult GetResetPasswordView([FromQuery] ResetPasswordRequestViewModel viewModel)
        {
            if (viewModel is null)
                throw new ArgumentNullException(nameof(viewModel));

            if (!ModelState.IsValid)
                return RedirectToAction("Index", "Account");

            var model = new ResetPasswordViewModel
            {
                Code = viewModel.Code,
                UserId = viewModel.UserId,
            };

            return View("ResetPassword", model);
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route(EndpointConstants.IdentityResetPassword)]
        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordViewModel viewModel)
        {
            if (viewModel is null)
                throw new ArgumentNullException(nameof(viewModel));

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Password", "Please be sure to enter a password that follows the requirements.");
                return View("ResetPassword", viewModel);
            }

            try
            {
                await _identityService.ResetUserPasswordAsync(viewModel.UserId, viewModel.Code, viewModel.Password)
                    .ConfigureAwait(false);
            }
            catch (Exception)
            {
                ModelState.AddModelError("Password", "Please be sure to enter a password that follows the requirements.");
                return View("ResetPassword", viewModel);
            }

            var messageViewModel = new MessageViewModel
            {
                Title = "Password changed",
                Message = "Password changed correctly, please log in again.",
            };

            return View("Message", messageViewModel);
        }
    }
}
