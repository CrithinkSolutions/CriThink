using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Security.Claims;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using CriThink.Server.Application.Commands;
using CriThink.Server.Application.Queries;
using CriThink.Server.Infrastructure.ExtensionMethods;
using CriThink.Server.Web.ActionFilters;
using CriThink.Server.Web.Models.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authentication;
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
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class IdentityController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IIdentityQueries _identityQueries;
        private readonly ILogger<IdentityController> _logger;

        public IdentityController(
            IMediator mediator,
            IIdentityQueries identityQueries,
            ILogger<IdentityController> logger)
        {
            _mediator = mediator ??
                throw new ArgumentNullException(nameof(mediator));

            _identityQueries = identityQueries ??
                throw new ArgumentNullException(nameof(identityQueries));
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
        ///         "email": "email",
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
        [Consumes("multipart/form-data")]
        [HttpPost]
        public async Task<IActionResult> SignUpUserAsync(
            [FromForm] UserSignUpRequest request,
            [FileSize(5 * 1024, 3 * 1024 * 1024)]
            [AllowedExtensions(new [] { ".jpg", ".jpeg" })]
            IFormFile formFile)
        {
            var command = new CreateUserCommand(
                request.Username,
                request.Email,
                request.Password,
                formFile);

            var response = await _mediator.Send(command);
            return Ok(new ApiOkResponse(response));
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
        ///         "username": "username",
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
        [HttpPost]
        public async Task<IActionResult> LoginUserAsync([FromBody] UserLoginRequest request)
        {
            var command = new LoginJwtUserCommand(
                request.Email,
                request.Username,
                request.Password);

            var response = await _mediator.Send(command);
            return Ok(new ApiOkResponse(response));
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
        [HttpPost]
        public async Task<IActionResult> RefreshUserTokenAsync([FromBody] UserRefreshTokenRequest request)
        {
            var command = new ExchangeRefreshTokenCommand(
                request.AccessToken,
                request.RefreshToken);

            var response = await _mediator.Send(command);
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
            var command = new VerifyAccountEmailCommand(
                request.UserId,
                request.Code);

            var response = await _mediator.Send(command);

            return View("EmailConfirmation", response is not null);
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
        /// <param name="request">User id and the code</param>
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
        public async Task<IActionResult> ConfirmEmailFromMobileAsync([FromBody] EmailConfirmationRequest request)
        {
            var command = new VerifyAccountEmailCommand(
                request.UserId,
                request.Code);

            var response = await _mediator.Send(command);
            return Ok(new ApiOkResponse(response));
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
        [HttpPost]
        public async Task<IActionResult> ChangeUserPasswordAsync([FromBody] ChangePasswordRequest dto)
        {
            var userId = User.GetId();

            var command = new UpdatePasswordCommand(
                userId,
                dto.CurrentPassword,
                dto.NewPassword);

            await _mediator.Send(command);

            return NoContent();
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
        [HttpPost]
        public async Task<IActionResult> RequestTemporaryTokenAsync([FromBody] ForgotPasswordRequest dto)
        {
            var command = new ForgotPasswordCommand(
                dto.Email, dto.UserName);

            await _mediator.Send(command);
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
        [HttpPost]
        public async Task<IActionResult> ResetUserPasswordAsync([FromBody] ResetPasswordRequest dto)
        {
            var command = new ResetUserPasswordCommand(
                dto.UserId,
                dto.Token,
                dto.NewPassword);

            await _mediator.Send(command);

            return NoContent();
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
        [HttpPost]
        public async Task<IActionResult> ExternalProviderLogin([FromBody] ExternalLoginProviderRequest dto)
        {
            var command = new LoginJwtUsingExternalProviderCommand(
                dto.SocialProvider,
                dto.UserToken);

            var response = await _mediator.Send(command);

            return Ok(new ApiOkResponse(response));
        }

        [AllowAnonymous]
        [Route(EndpointConstants.IdentityExternalLogin + "/{scheme}")]
        [ProducesResponseType(typeof(UserLoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiBadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status503ServiceUnavailable)]
        [HttpGet]
        public async Task SocialLogin([FromRoute] string scheme)
        {
            const string Callback = "xamarinapp";
            var auth = await Request.HttpContext.AuthenticateAsync(scheme);

            _logger?.LogWarning(
                "Auth done with result {succeed} with scheme {scheme}",
                auth.Succeeded,
                scheme);

            if (!auth.Succeeded
                || auth?.Principal == null
                || !auth.Principal.Identities.Any(id => id.IsAuthenticated)
                || string.IsNullOrEmpty(auth.Properties.GetTokenValue("access_token")))
            {
                _logger?.LogWarning(
                    "Auth failed, challenging. Host: {host}; Scheme: {scheme}",
                    Request.Host,
                    Request.Scheme);

                // Not authenticated, challenge
                //Request.Host = new HostString("localhost", 5001);
                //Request.Scheme = "https";

                var properties = new AuthenticationProperties
                {
                    RedirectUri = Url.Action("Test", "Identity")
                };

                await Request.HttpContext.ChallengeAsync(scheme, properties);
            }
            else
            {
                _logger?.LogWarning("Auth succeeded");

                var claims = auth.Principal.Identities.FirstOrDefault()?.Claims;

                var email = string.Empty;
                email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var givenName = claims?.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;
                var surName = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value;
                var nameIdentifier = claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                string picture = string.Empty;

                _logger?.LogWarning(
                            "Claims got: {email}; {givenName}; {surname}; {name}",
                    email,
                    givenName,
                    surName,
                    nameIdentifier);

                var command = new LoginJwtUserCommand("andrea.grillo@outlook.com", null, "king2Pac!");

                var response = await _mediator.Send(command);

                _logger?.LogWarning("Login done");

                var qs = new Dictionary<string, string>
                {
                    { "access_token", response.JwtToken.Token },
                    { "refresh_token",  response.RefreshToken },
                    { "jwt_token_expires", response.JwtToken.ExpirationDate.Ticks.ToString() },
                    { "email", "test@email.it" },
                    { "firstName", "andrea" },
                    { "picture", null },
                    { "secondName", "grillo" },
                };

                var url = Callback + "://#" + string.Join(
                        "&",
                        qs.Where(kvp => !string.IsNullOrEmpty(kvp.Value) && kvp.Value != "-1")
                        .Select(kvp => $"{WebUtility.UrlEncode(kvp.Key)}={WebUtility.UrlEncode(kvp.Value)}"));

                _logger?.LogWarning("Callback: {url}", url);

                // Redirect to final url
                Request.HttpContext.Response.Redirect(url);
            }
        }

        [AllowAnonymous]
        [Route("signin-google")]
        [HttpGet]
        public async Task Test()
        {
            _logger?.LogWarning("alternative method");
            const string Callback = "xamarinapp";

            var auth = await Request.HttpContext.AuthenticateAsync("Google");

            _logger?.LogWarning(
                "Auth done with result {succeed}",
                auth.Succeeded);

            var claims = auth.Principal.Identities.FirstOrDefault()?.Claims;

            var email = string.Empty;
            email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var givenName = claims?.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;
            var surName = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value;
            var nameIdentifier = claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            string picture = string.Empty;

            _logger?.LogWarning(
                "Claims got: {email}; {givenName}; {surname}; {name}",
                email,
                givenName,
                surName,
                nameIdentifier);

            var command = new LoginJwtUserCommand("andrea.grillo@outlook.com", null, "king2Pac!");

            var response = await _mediator.Send(command);

            _logger?.LogWarning("Login done");

            var qs = new Dictionary<string, string>
                {
                    { "access_token", response.JwtToken.Token },
                    { "refresh_token",  response.RefreshToken },
                    { "jwt_token_expires", response.JwtToken.ExpirationDate.Ticks.ToString() },
                    { "email", "test@email.it" },
                    { "firstName", "andrea" },
                    { "picture", null },
                    { "secondName", "grillo" },
                };

            var url = Callback + "://#" + string.Join(
                    "&",
                    qs.Where(kvp => !string.IsNullOrEmpty(kvp.Value) && kvp.Value != "-1")
                    .Select(kvp => $"{WebUtility.UrlEncode(kvp.Key)}={WebUtility.UrlEncode(kvp.Value)}"));

            _logger?.LogWarning("Callback: {url}", url);

            // Redirect to final url
            Request.HttpContext.Response.Redirect(url);
        }

        /// <summary>
        /// Checks if the given username is been already taken by another user
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET: /api/identity/username-availability?username=username
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
        [HttpGet]
        public async Task<IActionResult> GetUsernameAvailabilityAsync([FromQuery] UsernameAvailabilityRequest dto)
        {
            var response = await _identityQueries.AnyUserByUsernameAsync(dto.Username);
            return Ok(new ApiOkResponse(response));
        }

        /// <summary>
        /// Delete current user. Returns the scheduled deletion time
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     DELETE: /api/identity/user
        ///     
        /// </remarks>
        /// <response code="200">Returns the scheduled deletion time</response>
        /// <response code="400">If the request body is invalid</response>
        /// <response code="500">If the server can't process the request</response>
        /// <response code="503">If the server is not ready to handle the request</response>
        [Route(EndpointConstants.IdentityDeleteUser)]
        [ProducesResponseType(typeof(UserSoftDeletionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status503ServiceUnavailable)]
        [HttpDelete]
        public async Task<IActionResult> DeleteUserAsync()
        {
            var userId = User.GetId();

            var command = new DeleteUserCommand(userId);

            var response = await _mediator.Send(command);
            return Ok(new ApiOkResponse(response));
        }

        /// <summary>
        /// Restore a previously deleted account. The user
        /// will receive an email to restore the password.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PATCH: /api/identity/restore-user
        ///     {
        ///         "email": "email",
        ///         "username": "username"
        ///     }
        ///     
        /// </remarks>
        /// <param name="dto">Request with user email</param>
        /// <response code="204">Returns when operation succeeds</response>
        /// <response code="400">If the request body is invalid</response>
        /// <response code="500">If the server can't process the request</response>
        /// <response code="503">If the server is not ready to handle the request</response>
        [AllowAnonymous]
        [Route(EndpointConstants.IdentityRestoreUser)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status503ServiceUnavailable)]
        [HttpPatch]
        public async Task<IActionResult> RestoreUserAsync([FromBody] RestoreUserRequest dto)
        {
            var command = new RestoreUserCommand(
                dto.Email,
                dto.Username);

            await _mediator.Send(command);

            return NoContent();
        }
    }
}
