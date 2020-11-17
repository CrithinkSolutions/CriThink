using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Amazon.SecretsManager.Model;
using CriThink.Common.Endpoints;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using CriThink.Common.Helpers;
using CriThink.Server.Core.Interfaces;
using CriThink.Server.Web.ActionFilters;
using CriThink.Server.Web.Models.DTOs;
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
        /// Register a new user and send an email with confirmation code
        /// </summary>
        /// <returns>If successfull, returns the JWT token</returns>
        [AllowAnonymous]
        [Route(EndpointConstants.IdentitySignUp)] // api/identity/sign-up
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        [Route(EndpointConstants.IdentityLogin)] // api/identity/login
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> LoginUserAsync([FromBody] UserLoginRequest request)
        {
            try
            {
                var response = await _identityService.LoginUserAsync(request).ConfigureAwait(false);
                return Ok(new ApiOkResponse(response));
            }
            catch (Core.Exceptions.ResourceNotFoundException)
            {
                throw new Core.Exceptions.ResourceNotFoundException("The user or the password are incorrect");
            }
        }

        /// <summary>
        /// Confirm the user email, enabling the account. Generally called when
        /// user click on the email link
        /// </summary>
        /// <param name="userId">The user id</param>
        /// <param name="code">The code generated during the registration</param>
        /// <returns>Returns the confirmation of success</returns>
        [AllowAnonymous]
        [Route(EndpointConstants.IdentityConfirmEmail)] // api/identity/confirm-email?{userId}&{code}
        [HttpGet]
        public async Task<IActionResult> ConfirmEmailAsync(string userId, string code)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("User Id can't be null");
            if (string.IsNullOrWhiteSpace(code))
                return BadRequest("Code can't be null");

            string decodedCode;
            try
            {
                decodedCode = Base64Helper.FromBase64(code);
            }
            catch (FormatException)
            {
                return BadRequest("The given code is not valid");
            }

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
        [Route(EndpointConstants.IdentityChangePassword)] // api/identity/change-password
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> ChangeUserPasswordAsync([FromBody] ChangePasswordRequest dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userEmail))
                return Forbid();

            try
            {
                var result = await _identityService
                    .ChangeUserPasswordAsync(userEmail, dto.CurrentPassword, dto.NewPassword)
                    .ConfigureAwait(false);

                if (result)
                    return Ok();
            }
            catch (Core.Exceptions.ResourceNotFoundException)
            {
                throw new Core.Exceptions.ResourceNotFoundException("The provided passwords for the given user are incorrect");
            }

            return BadRequest();
        }

        /// <summary>
        /// Request a temporary token to reset the forgot password
        /// </summary>
        /// <param name="dto">Email or the username of the account owner</param>
        /// <returns>Send an email with the temporary code</returns>
        [AllowAnonymous]
        [Route(EndpointConstants.IdentityForgotPassword)] // api/identity/forgot-password
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> RequestTemporaryTokenAsync([FromBody] ForgotPasswordRequest dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            try
            {
                await _identityService.GenerateUserPasswordTokenAsync(dto.Email, dto.UserName).ConfigureAwait(false);
            }
            catch (Core.Exceptions.ResourceNotFoundException)
            {
                throw new Core.Exceptions.ResourceNotFoundException("The provided email or username are incorrect");
            }

            return NoContent();
        }

        /// <summary>
        /// Reset the user password, replacing it with a new one
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route(EndpointConstants.IdentityResetPassword)] // api/identity/reset-password
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> ResetUserPasswordAsync([FromBody] ResetPasswordRequest dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            string decodedCode;

            try
            {
                decodedCode = Base64Helper.FromBase64(dto.Token);
            }
            catch (FormatException)
            {
                return BadRequest(new ApiBadRequestResponse(nameof(dto.Token), new[] { "The given code is not valid" }));
            }

            try
            {
                var response = await _identityService.ResetUserPasswordAsync(dto.UserId, decodedCode, dto.NewPassword)
                    .ConfigureAwait(false);
                return Ok(new ApiOkResponse(response));
            }
            catch (Core.Exceptions.ResourceNotFoundException)
            {
                throw new ResourceNotFoundException("The provided user, token or the password are incorrect");
            }
        }

        /// <summary>
        /// Log the user via an external provider
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route(EndpointConstants.IdentityExternalLogin)] // api/identity/external-login
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> ExternalProviderLogin([FromBody] ExternalLoginProviderRequest dto)
        {
            var response = await _identityService.ExternalProviderLoginAsync(dto).ConfigureAwait(false);

            return Ok(new ApiOkResponse(response));
        }
    }
}
