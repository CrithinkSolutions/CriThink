using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Common.Endpoints.DTOs.UserProfile;
using CriThink.Server.Core.Interfaces;
using CriThink.Server.Web.ActionFilters;
using CriThink.Server.Web.Models.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CriThink.Server.Web.Controllers
{
    /// <summary>
    /// This controller contains APIs to update the user profile
    /// </summary>
    [ApiVersion(EndpointConstants.VersionOne)]
    [ApiValidationFilter]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route(EndpointConstants.ApiBase + EndpointConstants.UserProfileBase)]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _profileService;

        public UserProfileController(IUserProfileService profileService)
        {
            _profileService = profileService ?? throw new ArgumentNullException(nameof(profileService));
        }

        /// <summary>
        /// Update the user information
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PATCH: /api/user-profile/
        ///     {
        ///         "givenName": "givenName",
        ///         "familyName": "familyName"
        ///     }
        /// 
        /// </remarks>
        /// <response code="204">Returns when operation succeeds</response>
        /// <response code="401">If the user is not authorized</response>
        /// <response code="500">If the server can't process the request</response>
        /// <response code="503">If the server is not ready to handle the request</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status503ServiceUnavailable)]
        [Produces("application/json")]
        [HttpPatch]
        public async Task<IActionResult> UpdateUserProfileAsync([Required] UserProfileUpdateRequest request)
        {
            await _profileService.UpdateUserProfileAsync(request).ConfigureAwait(false);
            return NoContent();
        }

        /// <summary>
        /// Updates the user avatar. File size must be between 5kb and 2mb
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PATCH: /api/user-profile/upload-avatar
        ///  
        /// </remarks>
        /// <param name="formFile">Image to use as avatar. Between 5kb and 3mb. Only
        /// jpg and jpeg allowed</param>
        /// <response code="204">Returns when operation succeeds</response>
        /// <response code="400">If the request is invalid</response>
        /// <response code="500">If the server can't process the request</response>
        /// <response code="503">If the server is not ready to handle the request</response>
        [Route(EndpointConstants.UserProfileUploadAvatar)]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiBadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status503ServiceUnavailable)]
        [Produces("application/json")]
        [HttpPatch]
        public async Task<IActionResult> UploadAvatarAsync(
            [FileSize(5 * 1024, 3 * 1024 * 1024)]
            [AllowedExtensions(new [] { ".jpg", ".jpeg" })]
            [Required]
            IFormFile formFile)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            await _profileService.UpdateUserAvatarAsync(formFile, Guid.Parse(userId));
            return NoContent();
        }

        /// <summary>
        /// Get full user details
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET: /api/user-profile/
        /// 
        /// </remarks>
        /// <response code="200">Returns full user details</response>
        /// <response code="401">If the user is not authorized</response>
        /// <response code="500">If the server can't process the request</response>
        /// <response code="503">If the server is not ready to handle the request</response>
        [ProducesResponseType(typeof(UserProfileGetResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status503ServiceUnavailable)]
        [Produces("application/json")]
        [HttpGet]
        public async Task<IActionResult> GetUserDetailsAsync()
        {
            var response = await _profileService.GetUserProfileAsync().ConfigureAwait(false);
            return Ok(new ApiOkResponse(response));
        }
    }
}
