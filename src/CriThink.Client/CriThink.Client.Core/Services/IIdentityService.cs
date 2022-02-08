using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Models.Identity;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using Refit;

namespace CriThink.Client.Core.Services
{
    public interface IIdentityService
    {
        /// <summary>
        /// Get user credentials to access APIs
        /// </summary>
        /// <returns>User access info</returns>
        Task<UserAccess> GetLoggedUserAccessAsync();

        /// <summary>
        /// Performs login
        /// </summary>
        /// <param name="request">User data</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>Login response data</returns>
        Task<UserLoginResponse> PerformLoginAsync(UserLoginRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Performs login or sign in using an external social provider
        /// </summary>
        /// <param name="request">User and social provider data</param>
        /// <param name="cancellationToken">(Optional) Cancellation token to cancel the operation</param>
        /// <returns>Login response data</returns>
        Task<UserLoginResponse> PerformSocialLoginSignInAsync(ExternalLoginProviderRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Exchange access and refresh tokens for a new pair. New values are
        /// stored locally replacing the old ones
        /// </summary>
        /// <param name="cancellationToken">(Optional) Cancellation token</param>
        /// <returns>The new access and refresh tokens</returns>
        Task<UserRefreshTokenResponse> ExchangeTokensAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Requests a temporary token to restore the password
        /// </summary>
        /// <param name="request">Email or the username of the account owner</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>Operation status result</returns>
        Task RequestTemporaryTokenAsync(ForgotPasswordRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Change the current and forgotten password with the given one
        /// </summary>
        /// <param name="request">User id, temporary token and new password</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>User info and token</returns>
        Task<VerifyUserEmailResponse> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Performs user signup
        /// </summary>
        /// <param name="request">User data</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>Registration response data</returns>
        Task<UserSignUpResponse> PerformSignUpAsync(UserSignUpRequest request, StreamPart streamPart, CancellationToken cancellationToken);

        /// <summary>
        /// Verify user email
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="code">Code contained into the email</param>
        /// <returns>UserInfo and the token</returns>
        Task<VerifyUserEmailResponse> ConfirmUserEmailAsync(string userId, string code);

        /// <summary>
        /// Perform logout and erase all user information
        /// </summary>
        void PerformLogout();

        /// <summary>
        /// Request for user account deletion
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>Scheduled deletion date</returns>
        Task<UserSoftDeletionResponse> DeleteAccountAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Restore a previously deleted account
        /// </summary>
        /// <param name="request">Email or username</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns></returns>
        Task RestoreDeletedAccountAsync(RestoreUserRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Check if the given username is available or not
        /// </summary>
        /// <param name="username">Username to check</param>
        /// <param name="cancellationToken">(Optional) Cancellation token to cancel the operation</param>
        /// <returns>True if the username is available, false if not</returns>
        Task<bool> CheckForUsernameAvailabilityAsync(string username, CancellationToken cancellationToken = default);
    }
}