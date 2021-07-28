using System;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Server.Application.Commands;
using CriThink.Server.Web.Areas.Publics.ViewModel;
using CriThink.Server.Web.Areas.Publics.ViewModel.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CriThink.Server.Web.Areas.Publics.Controllers
{
    [Area("Public")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class IdentityController : Controller
    {
        private readonly IMediator _mediator;

        public IdentityController(IMediator mediator)
        {
            _mediator = mediator ??
                throw new ArgumentNullException(nameof(mediator));
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
                var command = new ResetUserPasswordCommand(
                    viewModel.UserId,
                    viewModel.Code,
                    viewModel.Password);

                await _mediator.Send(command);
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
