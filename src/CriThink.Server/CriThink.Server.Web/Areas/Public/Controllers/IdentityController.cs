using System;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Common.Helpers;
using CriThink.Server.Web.Areas.Public.ViewModel.Identity;
using CriThink.Server.Web.Areas.Public.ViewModel.Shared;
using CriThink.Server.Web.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Web.Areas.Public.Controllers
{
    [Area("Public")]
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

            var decodedCode = Base64Helper.FromBase64(viewModel.Code);

            var result = await _identityService.ResetUserPasswordAsync(viewModel.UserId, decodedCode, viewModel.Password).ConfigureAwait(false);

            if (result)
            {
                var messageViewModel = new MessageViewModel
                {
                    Title = "Password changed",
                    Message = "Password changed correctly, please log in again.",
                };
                return View("Message", messageViewModel);
            }
            else
            {
                ModelState.AddModelError("Password", "Please be sure to enter a password that follows the requirements.");
                return View("ResetPassword", viewModel);
            }

        }
    }
}
