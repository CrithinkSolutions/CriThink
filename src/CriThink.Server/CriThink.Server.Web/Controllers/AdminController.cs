using System;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Common.Endpoints.DTOs.Admin;
using CriThink.Server.Web.ActionFilters;
using CriThink.Server.Web.Models.DTOs;
using CriThink.Server.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CriThink.Server.Web.Controllers
{
    /// <summary>
    /// This controller contains APIs accessible only to admins
    /// </summary>
    [ApiVersion(EndpointConstants.VersionOne)]
    [ApiValidationFilter]
    [ApiController]
    [Route(EndpointConstants.ApiBase + EndpointConstants.AdminBase)] //api/admin
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IIdentityService _identityService;

        public AdminController(IIdentityService identityService)
        {
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }

        /// <summary>
        /// Register a new admin and send an email with confirmation code
        /// </summary>
        /// <returns>If successfull, returns the JWT token</returns>
        [Route(EndpointConstants.AdminSignUp)] // api/admin/sign-up
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> SignUpAdminAsync([FromBody] AdminSignUpRequest request)
        {
            var creationResponse = await _identityService.CreateNewAdminAsync(request).ConfigureAwait(false);
            return Ok(new ApiOkResponse(creationResponse));
        }

        /// <summary>
        /// Get list of roles
        /// </summary>
        /// <returns>Returns roles</returns>
        [Route(EndpointConstants.AdminRole)] // api/admin/role
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpGet]
        public async Task<IActionResult> GetAllRolesAsync()
        {
            var roles = await _identityService.GetRolesAsync().ConfigureAwait(false);
            return Ok(new ApiOkResponse(roles));
        }

        /// <summary>
        /// Create a new user role
        /// </summary>
        /// <param name="request">Role name</param>
        /// <returns>Returns NoContent</returns>
        [Route(EndpointConstants.AdminRole)] // api/admin/role
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> AddRoleAsync([FromBody] SimpleRoleNameRequest request)
        {
            await _identityService.CreateNewRoleAsync(request).ConfigureAwait(false);
            return NoContent();
        }

        /// <summary>
        /// Delete a new user role
        /// </summary>
        /// <param name="request">Role name</param>
        /// <returns>Returns NoContent</returns>
        [Route(EndpointConstants.AdminRole)] // api/admin/role
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpDelete]
        public async Task<IActionResult> RemoveRoleAsync([FromBody] SimpleRoleNameRequest request)
        {
            await _identityService.DeleteNewRoleAsync(request).ConfigureAwait(false);
            return NoContent();
        }

        /// <summary>
        /// Rename an user role
        /// </summary>
        /// <param name="request">New role name</param>
        /// <returns>Returns NoContent</returns>
        [Route(EndpointConstants.AdminRole)] // api/admin/role
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpPatch]
        public async Task<IActionResult> UpdateRoleNameAsync([FromBody] RoleUpdateNameRequest request)
        {
            await _identityService.UpdateRoleNameAsync(request).ConfigureAwait(false);
            return NoContent();
        }
    }
}
