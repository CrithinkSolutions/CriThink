using System;
using System.Security.Claims;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using CriThink.Common.Helpers;
using CriThink.Server.Core.Exceptions;
using CriThink.Server.Core.Interfaces;
using CriThink.Server.Web.ActionFilters;
using CriThink.Server.Web.Models.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route(EndpointConstants.ApiBase + EndpointConstants.IdentityBase)]
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
        /// Register a new user and send their an email with confirmation code
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST: /api/identity/sign-up
        ///     {
        ///         "username": "username",
        ///         "password": "password",
        ///     }
        /// 
        /// </remarks>
        /// <param name="request">Request body with email/username and password</param>
        /// <response code="200">Returns the user confirmation code and id</response>
        /// <response code="400">If the request body is invalid</response>
        /// <response code="500">If the server can't process the request</response>
        /// <response code="503">If the server is not ready to handle the request</response>
        [AllowAnonymous]
        [Route(EndpointConstants.IdentitySignUp)]
        [ProducesResponseType(typeof(UserSignUpResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiBadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status503ServiceUnavailable)]
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
        /// <remarks>
        /// Sample request:
        ///
        ///     POST: /api/identity/login
        ///     {
        ///         "email": "email",
        ///         "password": "password",
        ///     }
        /// 
        /// </remarks>
        /// <param name="request">Request body with email/username and password</param>
        /// <response code="200">If successfull, returns user info and JWT token</response>
        /// <response code="400">If the request body is invalid</response>
        /// <response code="500">If the server can't process the request</response>
        /// <response code="503">If the server is not ready to handle the request</response>
        [AllowAnonymous]
        [Route(EndpointConstants.IdentityLogin)]
        [ProducesResponseType(typeof(UserLoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiBadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status503ServiceUnavailable)]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> LoginUserAsync([FromBody] UserLoginRequest request)
        {
            try
            {
                var response = await _identityService.LoginUserAsync(request).ConfigureAwait(false);
                return Ok(new ApiOkResponse(response));
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "User or password incorrect while login");
                throw new ResourceNotFoundException("The user or the password are incorrect");
            }
        }

        /// <summary>
        /// Confirm the user email, enabling the account. Generally called when
        /// user click on the email link
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET: /api/identity/confirm-email?{userId}{code}
        /// 
        /// </remarks>
        /// <param name="userId">The user id</param>
        /// <param name="code">The code generated during the registration</param>
        /// <response code="200">Returns the view</response>
        /// <response code="500">If the server can't process the request</response>
        /// <response code="503">If the server is not ready to handle the request</response>
        [AllowAnonymous]
        [Route(EndpointConstants.IdentityConfirmEmail)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status503ServiceUnavailable)]
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
        /// Confirm the user email from mobile, enabling the account. Generally called
        /// when the user clicks on the email link after the sign up using the app
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST: /api/identity/mobile/confirm-email
        ///     {
        ///         "userId": "userId",
        ///         "code": "code",
        ///     }
        /// 
        /// </remarks>
        /// <param name="dto">User id and the code</param>
        /// <response code="200">Returns the user info with the jwt token</response>
        /// <response code="400">If the request body is invalid</response>
        /// <response code="500">If the server can't process the request</response>
        /// <response code="503">If the server is not ready to handle the request</response>
        [AllowAnonymous]
        [Route(EndpointConstants.Mobile + EndpointConstants.IdentityConfirmEmail)]
        [ProducesResponseType(typeof(VerifyUserEmailResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiBadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status503ServiceUnavailable)]
        [HttpPost]
        public async Task<IActionResult> ConfirmEmailFromMobileAsync([FromBody] EmailConfirmationRequest dto)
        {
            if (dto is null)
                throw new ArgumentNullException(nameof(dto));

            if (string.IsNullOrWhiteSpace(dto.Code))
                return BadRequest("Code can't be null");

            string decodedCode;
            try
            {
                decodedCode = Base64Helper.FromBase64(dto.Code);
            }
            catch (FormatException)
            {
                return BadRequest("The given code is not valid");
            }

            try
            {
                var userInfo = await _identityService.VerifyAccountEmailAsync(dto.UserId.ToString(), decodedCode).ConfigureAwait(false);
                return Ok(new ApiOkResponse(userInfo));
            }
            catch (IdentityOperationException ex)
            {
                _logger?.LogError(ex, "Error confirming email from mobile");
                throw;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error confirming email from mobile");
                return BadRequest();
            }
        }

        /// <summary>
        /// Request for a new password
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST: /api/identity/change-password
        ///     {
        ///         "currentPassword": "currentPassword",
        ///         "newPassword": "newPassword",
        ///     }
        /// 
        /// </remarks>
        /// <param name="dto">Request with old and new password</param>
        /// <response code="204">Returns when operation succeeds</response>
        /// <response code="400">If the request body is invalid</response>
        /// <response code="401">If the user is not authorized</response>
        /// <response code="500">If the server can't process the request</response>
        /// <response code="503">If the server is not ready to handle the request</response>
        [Route(EndpointConstants.IdentityChangePassword)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiBadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status503ServiceUnavailable)]
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

                return result ?
                    NoContent() :
                    BadRequest();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error changing user password");
                return BadRequest();
            }
        }

        /// <summary>
        /// Request a temporary token to reset the forgot password, sent via email
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST: /api/identity/forgot-password
        ///     {
        ///         "email": "email",
        ///         "username": "username",
        ///     }
        /// 
        /// </remarks>
        /// <param name="dto">Email or the username of the account owner</param>
        /// <response code="204">Returns when operation succeeds</response>
        /// <response code="400">If the request body is invalid</response>
        /// <response code="500">If the server can't process the request</response>
        /// <response code="503">If the server is not ready to handle the request</response>
        [AllowAnonymous]
        [Route(EndpointConstants.IdentityForgotPassword)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiBadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status503ServiceUnavailable)]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> RequestTemporaryTokenAsync([FromBody] ForgotPasswordRequest dto)
        {
            if (dto is null)
                throw new ArgumentNullException(nameof(dto));

            try
            {
                await _identityService.GenerateUserPasswordTokenAsync(dto.Email, dto.UserName).ConfigureAwait(false);
            }
            catch (ResourceNotFoundException)
            {
                throw new ResourceNotFoundException("The provided email or username are incorrect");
            }

            return NoContent();
        }

        /// <summary>
        /// Reset the user password, replacing it with a new one
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST: /api/identity/reset-password
        ///     {
        ///         "userId": "userId",
        ///         "token": "token",
        ///         "newPassword": "newPassword",
        ///     }
        /// 
        /// </remarks>
        /// <param name="dto">The user id with the token received via email and the new password</param>
        /// <response code="204">Returns when operation succeeds</response>
        /// <response code="400">If the request body is invalid</response>
        /// <response code="500">If the server can't process the request</response>
        /// <response code="503">If the server is not ready to handle the request</response>
        [AllowAnonymous]
        [Route(EndpointConstants.IdentityResetPassword)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiBadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status503ServiceUnavailable)]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> ResetUserPasswordAsync([FromBody] ResetPasswordRequest dto)
        {
            if (dto is null)
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
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error resetting user password");
                return BadRequest();
            }
        }

        /// <summary>
        /// Log the user via an external provider (Facebook or Google)
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST: /api/identity/external-login
        ///     {
        ///         "socialProvider": "socialProvider",
        ///         "userToken": "userToken",
        ///     }
        /// 
        /// </remarks>
        /// <param name="dto">Social provider and its token</param>
        /// <response code="200">Returns the user info with the JWT token</response>
        /// <response code="400">If the request body is invalid</response>
        /// <response code="500">If the server can't process the request</response>
        /// <response code="503">If the server is not ready to handle the request</response>
        [AllowAnonymous]
        [Route(EndpointConstants.IdentityExternalLogin)]
        [ProducesResponseType(typeof(UserLoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiBadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status503ServiceUnavailable)]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> ExternalProviderLogin([FromBody] ExternalLoginProviderRequest dto)
        {
            var response = await _identityService.ExternalProviderLoginAsync(dto).ConfigureAwait(false);
            return Ok(new ApiOkResponse(response));
        }

        /// <summary>
        /// Checks if the given username is been already taken by another user
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST: /api/identity/username-availability
        ///     {
        ///         "username": "username",
        ///     }
        /// 
        /// </remarks>
        /// <param name="dto">The username to search</param>
        /// <response code="200">Returns a flag that is true if the username is not used yet</response>
        /// <response code="400">If the request body is invalid</response>
        /// <response code="500">If the server can't process the request</response>
        /// <response code="503">If the server is not ready to handle the request</response>
        [AllowAnonymous]
        [Route(EndpointConstants.IdentityUsernameAvailability)]
        [ProducesResponseType(typeof(UsernameAvailabilityResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiBadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status503ServiceUnavailable)]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> GetUsernameAvailabilityAsync([FromBody] UsernameAvailabilityRequest dto)
        {
            var response = await _identityService.GetUsernameAvailabilityAsync(dto).ConfigureAwait(false);
            return Ok(new ApiOkResponse(response));
        }
    }
}
