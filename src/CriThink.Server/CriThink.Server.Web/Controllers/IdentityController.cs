using System;
using System.Security.Claims;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Common.Helpers;
using CriThink.Server.Web.ActionFilters;
using CriThink.Server.Web.Models.DTOs;
using CriThink.Server.Web.Services;
using CriThink.Web.Models.DTOs.IdentityProvider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Web.Controllers
{
    /// <summary>
    /// This controller contains APIs to manage user identity
    /// </summary>
    [ApiVersion(EndpointConstants.VersionOne)]
    [ApiValidationFilter]
    [ApiController]
    [Route(EndpointConstants.ApiBase + EndpointConstants.IdentityBase)] //api/identity
    public class IdentityController : Controller
    {
        private readonly IIdentityService _identityService;
        private readonly ILogger<IdentityController> _logger;

        public IdentityController(IIdentityService identityService, ILogger<IdentityController> logger)
        {
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _logger = logger;
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <returns>If successfull, returns the JWT token</returns>
        [AllowAnonymous]
        [Route(EndpointConstants.SignUp)] // api/identity/sign-up
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> SignUpUserAsync([FromBody] UserSignUpRequest request)
        {
            var creationResponse = await _identityService.CreateNewUserAsync(request).ConfigureAwait(false);
            return Ok(new ApiOkResponse(new { userId = creationResponse.UserId, userEmail = creationResponse.UserEmail }));
        }

        /// <summary>
        /// Perform the user login
        /// </summary>
        /// <returns>If successfull, returns user info and JWT token</returns>
        [AllowAnonymous]
        [Route(EndpointConstants.Login)] // api/identity/login
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> LoginUserAsync([FromBody] UserLoginRequest request)
        {
            var response = await _identityService.LoginUserAsync(request).ConfigureAwait(false);
            return Ok(new ApiOkResponse(response));
        }

        /// <summary>
        /// Confirm the user email, enabling the account. Generally called when
        /// user click on the email link
        /// </summary>
        /// <param name="userId">The user id</param>
        /// <param name="code">The code generated during the registration</param>
        /// <returns>Returns the confirmation of success</returns>
        [AllowAnonymous]
        [Route(EndpointConstants.ConfirmEmail)] // api/identity/confirm-email
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("User Id can't be null");
            if (string.IsNullOrWhiteSpace(code))
                return BadRequest("Code can't be null");

            var decodedCode = Base64Helper.FromBase64(code);

            var result = true;

            try
            {
                await _identityService.VerifyAccountEmailAsync(userId, decodedCode).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error confirming email");
                result = false;
            }

            return View("EmailConfirmation", result);
        }

        /// <summary>
        /// Request for a new password
        /// </summary>
        /// <param name="dto">Request with old and new password</param>
        /// <returns>Returns HTTP status code</returns>
        [Authorize]
        [Route(EndpointConstants.ChangePassword)] // api/identity/change-password
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> ChangeUserPassword([FromBody] ChangePasswordRequest dto)
        {
            var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userEmail))
                return Forbid();

            var result = await _identityService.ChangeUserPasswordAsync(userEmail, dto.CurrentPassword, dto.NewPassword)
                .ConfigureAwait(false);

            if (result)
                return Ok();

            return BadRequest();
        }
    }
}
