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
        private readonly IDebunkNewsService _debunkingNewsService;

        public AdminController(IIdentityService identityService, IDebunkNewsService debunkingNewsService)
        {
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _debunkingNewsService = debunkingNewsService ?? throw new ArgumentNullException(nameof(debunkingNewsService));
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

        /// <summary>
        /// Update the role of the given user
        /// </summary>
        /// <param name="request">User and the new role</param>
        /// <returns>Returns NoContent</returns>
        [Route(EndpointConstants.AdminUserRole)] // api/admin/user/role
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpPatch]
        public async Task<IActionResult> UpdateUserRoleAsync([FromBody] UserRoleUpdateRequest request)
        {
            await _identityService.UpdateUserRoleAsync(request).ConfigureAwait(false);
            return NoContent();
        }

        /// <summary>
        /// Remove the given role from the given user
        /// </summary>
        /// <param name="request">User and the new role</param>
        /// <returns>Returns NoContent</returns>
        [Route(EndpointConstants.AdminUserRole)] // api/admin/user/role
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpDelete]
        public async Task<IActionResult> RemoveRoleFromUserAsync([FromBody] UserRoleUpdateRequest request)
        {
            await _identityService.RemoveRoleFromUserAsync(request).ConfigureAwait(false);
            return NoContent();
        }

        /// <summary>
        /// Get all the users paginated
        /// </summary>
        /// <param name="request">Page index and users per page</param>
        /// <returns>Returns list of users</returns>
        [Route(EndpointConstants.AdminUserGetAll)] // api/admin/user/all
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpGet]
        public async Task<IActionResult> GetAllUserAsync([FromQuery] UserGetAllRequest request)
        {
            var users = await _identityService.GetAllUsersAsync(request).ConfigureAwait(false);
            return Ok(new ApiOkResponse(users));
        }

        /// <summary>
        /// Get all the users paginated
        /// </summary>
        /// <param name="request">User id</param>
        /// <returns>Returns the user details</returns>
        [Route(EndpointConstants.AdminUser)] // api/admin/user
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpGet]
        public async Task<IActionResult> GetAllUserAsync([FromQuery] UserGetRequest request)
        {
            var user = await _identityService.GetUserByIdAsync(request).ConfigureAwait(false);
            return Ok(new ApiOkResponse(user));
        }

        /// <summary>
        /// Update the given user
        /// </summary>
        /// <param name="request">New user properties</param>
        /// <returns>Returns NoContent</returns>
        [Route(EndpointConstants.AdminUser)] // api/admin/user
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpPut]
        public async Task<IActionResult> UpdateUserAsync([FromBody] UserUpdateRequest request)
        {
            await _identityService.UpdateUserAsync(request).ConfigureAwait(false);
            return NoContent();
        }

        /// <summary>
        /// Mark the given user as deleted
        /// </summary>
        /// <param name="request">User id</param>
        /// <returns>Returns NoContent</returns>
        [Route(EndpointConstants.AdminUser)] // api/admin/user
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpPatch]
        public async Task<IActionResult> SoftDeleteUserAsync([FromBody] UserGetRequest request)
        {
            await _identityService.SoftDeleteUserAsync(request).ConfigureAwait(false);
            return NoContent();
        }

        /// <summary>
        /// Delete the user permanently
        /// </summary>
        /// <param name="request">User id</param>
        /// <returns>Returns NoContent</returns>
        [Route(EndpointConstants.AdminUser)] // api/admin/user
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpDelete]
        public async Task<IActionResult> DeleteUserAsync([FromBody] UserGetRequest request)
        {
            await _identityService.DeleteUserAsync(request).ConfigureAwait(false);
            return NoContent();
        }

        /// <summary>
        /// Adds the given debunking news. The given values overwrites the scraped data
        /// </summary>
        /// <param name="request">The debunking news</param>
        /// <returns></returns>
        [Route(EndpointConstants.AdminDebunkingNews)] // api/admin/debunking-news
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> AddDebunkingNewsAsync([FromBody] DebunkingNewsAddRequest request)
        {
            await _debunkingNewsService.AddDebunkingNewsAsync(request).ConfigureAwait(false);
            return NoContent();
        }

        [Route(EndpointConstants.AdminDebunkingNews)] // api/admin/debunking-news
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpDelete]
        public async Task<IActionResult> DeleteDebunkingNewsAsync([FromBody] SimpleDebunkingNewsRequest request)
        {
            await _debunkingNewsService.DeleteDebunkingNewsAsync(request).ConfigureAwait(false);
            return NoContent();
        }
    }
}
