using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace CriThink.Server.Core.Interfaces
{
    public interface IUserRepository
    {
        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="user">User data</param>
        /// <param name="plainPassword">(Optional) The desired user password. Do not
        /// pass if using an external login provider</param>
        /// <returns>An <see cref="IdentityResult"/></returns>
        Task<IdentityResult> CreateUserAsync(User user, string plainPassword = null);

        /// <summary>
        /// Generates a token to confirm the user email
        /// </summary>
        /// <param name="user">User that needs email verification</param>
        /// <returns>The base 64 encoded confirmation token</returns>
        Task<string> GetEmailConfirmationTokenAsync(User user);

        /// <summary>
        /// Confirm the user email using the token previously generated
        /// </summary>
        /// <param name="user">User that needs confirmation</param>
        /// <param name="encodedConfirmationCode">The base 64 encoded token</param>
        /// <returns>An <see cref="IdentityResult"/></returns>
        Task<IdentityResult> ConfirmUserEmailAsync(User user, string encodedConfirmationCode);

        /// <summary>
        /// Adds the given user to the given role category
        /// </summary>
        /// <param name="user">User to add</param>
        /// <param name="roleName">User role</param>
        /// <returns>An <see cref="IdentityResult"/></returns>
        Task<IdentityResult> AddUserToRoleAsync(User user, string roleName);

        /// <summary>
        /// Adds the given claims list to the given user
        /// </summary>
        /// <param name="user">User where to add claims</param>
        /// <param name="claims">Claims to add</param>
        /// <returns>An <see cref="IdentityResult"/></returns>
        Task<IdentityResult> AddClaimsToUserAsync(User user, IList<Claim> claims);

        /// <summary>
        /// Get the given user claims
        /// </summary>
        /// <param name="user">User which have claims to return</param>
        /// <returns>The <see cref="List{Claim}"/></returns>
        Task<IList<Claim>> GetUserClaimsAsync(User user);

        /// <summary>
        /// Get the user roles
        /// </summary>
        /// <param name="user">User to get roles</param>
        /// <returns>An <see cref="IList{T}"/></returns>
        Task<IList<string>> GetUserRolesAsync(User user);

        /// <summary>
        /// Revoke the given roles to the given user
        /// </summary>
        /// <param name="user">User from where remove roles</param>
        /// <param name="roles">Roles to remove</param>
        /// <returns>An <see cref="IdentityResult"/></returns>
        Task<IdentityResult> RemoveUserFromRolesAsync(User user, IList<string> roles);

        /// <summary>
        /// Revoke the given role to the given user
        /// </summary>
        /// <param name="user">User from where remove roles</param>
        /// <param name="role">Role to remove</param>
        /// <returns>An <see cref="IdentityResult"/></returns>
        Task<IdentityResult> RemoveUserFromRoleAsync(User user, string role);

        /// <summary>
        /// Get all users with pagination support
        /// </summary>
        /// <param name="pageSize">How many users return</param>
        /// <param name="pageIndex">Page index</param>
        /// <returns>A <see cref="List{User}"/></returns>
        Task<List<User>> GetAllUsersAsync(int pageSize, int pageIndex);

        /// <summary>
        /// Update the given user
        /// </summary>
        /// <param name="user">User to update</param>
        /// <returns>An <see cref="IdentityResult"/></returns>
        Task<IdentityResult> UpdateUserAsync(User user);

        /// <summary>
        /// Match the user password
        /// </summary>
        /// <param name="user">User to match</param>
        /// <param name="password">Password to verify</param>
        /// <returns>A <see cref="PasswordVerificationResult"/></returns>
        PasswordVerificationResult VerifyUserPassword(User user, string password);

        /// <summary>
        /// Change the password of the given user
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="currentPassword">The current user password</param>
        /// <param name="newPassword">The new user password</param>
        /// <returns>An <see cref="IdentityResult"/></returns>
        Task<IdentityResult> ChangeUserPasswordAsync(User user, string currentPassword, string newPassword);

        /// <summary>
        /// Generates a temporary token for the given user to reset
        /// the password
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>An <see cref="IdentityResult"/></returns>
        Task<string> GenerateUserPasswordResetTokenAsync(User user);

        /// <summary>
        /// Reset the user password
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="resetToken">The token previously generated using
        /// <see cref="GenerateUserPasswordResetTokenAsync"/></param>
        /// <param name="password">The new password</param>
        /// <returns>An <see cref="IdentityResult"/></returns>
        Task<IdentityResult> ResetUserPasswordAsync(User user, string resetToken, string password);

        /// <summary>
        /// Find a user through the username or the email
        /// </summary>
        /// <param name="value">USername or email</param>
        /// <param name="includeTokens">A flag indicating if the tokens
        /// must be included in query</param>
        /// <param name="cancellationToken">(Optional) Cancellation token</param>
        /// <returns>An <see cref="IdentityResult"/></returns>
        Task<User> FindUserAsync(string value, bool includeTokens, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns a flag whether the given user has a password
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>An flag</returns>
        Task<bool> HasUserPasswordAsync(User user);

        /// <summary>
        /// Find user logged in using an external provider
        /// </summary>
        /// <param name="provider">The social provider</param>
        /// <param name="userId">The user id</param>
        /// <returns>A <see cref="User"/></returns>
        Task<User> FindUserByLoginAsync(string provider, string userId);

        /// <summary>
        /// Link a user to a specific external login
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="userLoginInfo">The external login data</param>
        /// <returns>An <see cref="IdentityResult"/></returns>
        Task<IdentityResult> AddUserLoginAsync(User user, UserLoginInfo userLoginInfo);

        /// <summary>
        /// Login the given user
        /// </summary>
        /// <param name="user">User to login</param>
        /// <param name="password">The password</param>
        /// <param name="isPersistent">A flag indicating whether the login should
        /// be persistent</param>
        /// <param name="lockoutOnFailure">A flag indicating whether the failure should
        /// trigger the lockout</param>
        /// <returns>A <see cref="SignInResult"/></returns>
        Task<SignInResult> PasswordSignInAsync(User user, string password, bool isPersistent, bool lockoutOnFailure);

        /// <summary>
        /// Sign in the user
        /// </summary>
        /// <param name="user">The user</param>
        /// <param name="isPersistent">A flag indicating whether the login should
        /// be persistent</param>
        /// <returns>An <see cref="Task"/></returns>
        Task SignInAsync(User user, bool isPersistent);
    }
}
