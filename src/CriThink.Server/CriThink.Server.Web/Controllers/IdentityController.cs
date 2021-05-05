using System;
using System.Security.Claims;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
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
        /// Register a new user and send their an email with confirmation code.
        /// An avatar image is optional.
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
        /// <param name="formFile">(Optional) Image to use as avatar. Between 5kb and 3mb.
        /// Only jpg and jpeg allowed</param>
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
        public async Task<IActionResult> SignUpUserAsync(
            [FromForm] UserSignUpRequest request,
            [FileSize(5 * 1024, 3 * 1024 * 1024)]
            [AllowedExtensions(new [] { ".jpg", ".jpeg" })]
            IFormFile formFile)
        {
            var creationResponse = await _identityService.CreateNewUserAsync(request, formFile).ConfigureAwait(false);
            return Ok(new ApiOkResponse(creationResponse));
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
            catch (ResourceNotFoundException ex)
            {
                _logger?.LogError(ex, "User or password incorrect while login");
            }
            catch (InvalidOperationException ex)
            {
                _logger?.LogError(ex, "User or password incorrect while login");
            }

            throw new ResourceNotFoundException("The user does not exist or the password is incorrect");
        }

        /// <summary>
        /// Refresh the user token
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST: /api/identity/refresh-token
        ///     {
        ///         "accessToken": "accessToken",
        ///         "refreshToken": "refreshToken",
        ///     }
        /// 
        /// </remarks>
        /// <param name="request">Request body with current access and refresh token</param>
        /// <response code="200">If successfull, returns a new access and refresh token</response>
        /// <response code="400">If the request body is invalid</response>
        /// <response code="403">If the givne refresh token is invalid or expired</response>
        /// <response code="500">If the server can't process the request</response>
        /// <response code="503">If the server is not ready to handle the request</response>
        [AllowAnonymous]
        [Route(EndpointConstants.IdentityRefreshToken)]
        [ProducesResponseType(typeof(UserRefreshTokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiBadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status503ServiceUnavailable)]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> RefreshUserTokenAsync([FromBody] UserRefreshTokenRequest request)
        {
            var response = await _identityService.ExchangeRefreshTokenAsync(request).ConfigureAwait(false);
            return Ok(new ApiOkResponse(response));
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
        /// <param name="request">The user id and the code generated during the
        /// registration</param>
        /// <response code="200">Returns the view</response>
        /// <response code="500">If the server can't process the request</response>
        /// <response code="503">If the server is not ready to handle the request</response>
        [AllowAnonymous]
        [Route(EndpointConstants.IdentityConfirmEmail)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status503ServiceUnavailable)]
        [HttpGet]
        public async Task<IActionResult> ConfirmEmailAsync([FromQuery] EmailConfirmationRequest request)
        {
            var result = true;

            try
            {
                await _identityService.VerifyAccountEmailAsync(request.UserId.ToString(), request.Code).ConfigureAwait(false);
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
            try
            {
                var userInfo = await _identityService.VerifyAccountEmailAsync(dto.UserId.ToString(), dto.Code).ConfigureAwait(false);
                return Ok(new ApiOkResponse(userInfo));
            }
            catch (ResourceNotFoundException) { }
            catch (InvalidOperationException) { }
            catch (IdentityOperationException) { }

            throw new ResourceNotFoundException("The user does not exist or the code is invalid");
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
            var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userEmail))
                return Unauthorized();

            try
            {

                await _identityService
                    .ChangeUserPasswordAsync(userEmail, dto.CurrentPassword, dto.NewPassword)
                    .ConfigureAwait(false);

                return NoContent();
            }
            catch (ResourceNotFoundException) { }
            catch (InvalidOperationException) { }

            throw new ResourceNotFoundException("The user does not exist or the code is invalid");
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
            try
            {
                await _identityService.GenerateUserPasswordTokenAsync(dto.Email, dto.UserName).ConfigureAwait(false);
                return NoContent();
            }
            catch (ResourceNotFoundException) { }
            catch (InvalidOperationException) { }

            throw new ResourceNotFoundException("The user does not exist");
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
            try
            {
                await _identityService.ResetUserPasswordAsync(dto.UserId, dto.Token, dto.NewPassword)
                    .ConfigureAwait(false);

                return NoContent();
            }
            catch (ResourceNotFoundException) { }
            catch (InvalidOperationException) { }

            throw new ResourceNotFoundException("The user does not exist or the code is invalid");
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
