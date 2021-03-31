using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.Admin;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;

namespace CriThink.Server.Core.Interfaces
{
    public interface IIdentityService
    {
        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="request">DTO with user information</param>
        /// <returns>The operation result</returns>
        Task<UserSignUpResponse> CreateNewUserAsync(UserSignUpRequest request);

        /// <summary>
        /// Create a new admin
        /// </summary>
        /// <param name="request">DTO with admin information</param>
        /// <returns>The operation result</returns>
        Task<AdminSignUpResponse> CreateNewAdminAsync(AdminSignUpRequest request);

        /// <summary>
        /// Gets all the roles
        /// </summary>
        /// <returns>Role list</returns>
        Task<IList<RoleGetResponse>> GetRolesAsync();

        /// <summary>
        /// Add a new identity role
        /// </summary>
        /// <param name="request">Role to add</param>
        /// <returns>An asynchronous result</returns>
        Task CreateNewRoleAsync(SimpleRoleNameRequest request);

        /// <summary>
        /// Delete an identity role
        /// </summary>
        /// <param name="request">Role to delete</param>
        /// <returns>An asynchronous result</returns>
        Task DeleteNewRoleAsync(SimpleRoleNameRequest request);

        /// <summary>
        /// Rename an identity role name
        /// </summary>
        /// <param name="request">New role name</param>
        /// <returns>An asynchronous result</returns>
        Task UpdateRoleNameAsync(RoleUpdateNameRequest request);

        /// <summary>
        /// Update the user's role
        /// </summary>
        /// <param name="request">User and role</param>
        /// <returns>An asynchronous result</returns>
        Task UpdateUserRoleAsync(UserRoleUpdateRequest request);

        /// <summary>
        /// Remove a role from the user
        /// </summary>
        /// <param name="request">User and role</param>
        /// <returns>An asynchronous result</returns>
        Task RemoveRoleFromUserAsync(UserRoleUpdateRequest request);

        /// <summary>
        /// Get all users
        /// </summary>
        /// <param name="request">Page index and users per page</param>
        /// <returns>Returns list of users</returns>
        Task<UserGetAllResponse> GetAllUsersAsync(UserGetAllRequest request);

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="request">User id</param>
        /// <returns>Returns user details</returns>
        Task<UserGetDetailsResponse> GetUserByIdAsync(UserGetRequest request);

        /// <summary>
        /// Update user properties
        /// </summary>
        /// <param name="request">New properties</param>
        /// <returns></returns>
        Task UpdateUserAsync(UserUpdateRequest request);

        /// <summary>
        /// Soft delete a user
        /// </summary>
        /// <param name="request">User id</param>
        /// <returns>The operation result</returns>
        Task SoftDeleteUserAsync(UserGetRequest request);

        /// <summary>
        /// Login the given user
        /// </summary>
        /// <param name="request">DTO with user information</param>
        /// <returns>The operation result. It contains the token if successful</returns>
        Task<UserLoginResponse> LoginUserAsync(UserLoginRequest request);

        /// <summary>
        /// Login the given user
        /// </summary>
        /// <param name="emailOrUsername">User email or username</param>
        /// <param name="password">User password</param>
        /// <param name="rememberMe">True to store login in the cookie</param>
        /// <returns>The operation result</returns>
        Task<ClaimsIdentity> LoginUserAsync(string emailOrUsername, string password, bool rememberMe);

        /// <summary>
        /// Verify the user email through the email link
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="confirmationCode"></param>
        /// <returns>The operation result</returns>
        Task<VerifyUserEmailResponse> VerifyAccountEmailAsync(string userId, string confirmationCode);

        /// <summary>
        /// Allow user to change its personal password
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="currentPassword">Current password</param>
        /// <param name="newPassword">New password</param>
        /// <returns>Returns true if the password is changed, otherwise false</returns>
        Task ChangeUserPasswordAsync(string email, string currentPassword, string newPassword);

        /// <summary>
        /// Generates a temporary token for the user, in order to reset its
        /// password
        /// </summary>
        /// <param name="email">(Optional) The user email</param>
        /// <param name="username">(Optional) The username</param>
        /// <returns>A <see cref="Task"/></returns>
        Task GenerateUserPasswordTokenAsync(string email, string username);

        /// <summary>
        /// Reset the user password using the temporary token
        /// </summary>
        /// <param name="userId">The user id</param>
        /// <param name="token">The user temporary token (encoded)</param>
        /// <param name="newPassword">The new password</param>
        /// <returns></returns>
        Task ResetUserPasswordAsync(string userId, string token, string newPassword);

        /// <summary>
        /// Login using an external provider
        /// </summary>
        /// <param name="request">Login information</param>
        /// <returns>An <see cref="UserLoginResponse"/></returns>
        Task<UserLoginResponse> ExternalProviderLoginAsync(ExternalLoginProviderRequest request);

        /// <summary>
        /// Get the username availability. Returns true if username is not already
        /// taken
        /// </summary>
        /// <param name="request">Username</param>
        /// <returns>Returns true if the username if available, false if not</returns>
        Task<UsernameAvailabilityResponse> GetUsernameAvailabilityAsync(UsernameAvailabilityRequest request);
    }
}
