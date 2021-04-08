using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Api;
using CriThink.Client.Core.Models.Identity;
using CriThink.Client.Core.Repositories;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using MvvmCross.Logging;
using Refit;

namespace CriThink.Client.Core.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IIdentityApi _identityApi;
        private readonly IIdentityRepository _identityRepository;
        private readonly IMvxLog _log;

        public IdentityService(IIdentityApi identityApi, IIdentityRepository identityRepository, IMvxLogProvider logProvider)
        {
            _identityApi = identityApi ?? throw new ArgumentNullException(nameof(identityApi));
            _identityRepository = identityRepository ?? throw new ArgumentNullException(nameof(identityRepository));
            _log = logProvider?.GetLogFor<IdentityService>();
        }

        public async Task<User> GetLoggedUserAsync()
        {
            try
            {
                var user = await _identityRepository.GetUserInfoAsync().ConfigureAwait(false);
                return user;
            }
            catch (Exception ex)
            {
                _log?.FatalException("Can't get user info", ex);
                return null;
            }
        }

        public async Task<string> GetUserTokenAsync()
        {
            try
            {
                var userToken = await _identityRepository.GetUserTokenAsync().ConfigureAwait(false);
                return userToken;
            }
            catch (Exception ex)
            {
                _log?.FatalException("Error getting user token", ex);
                return string.Empty;
            }
        }

        public async Task<UserLoginResponse> PerformLoginAsync(UserLoginRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            UserLoginResponse loginResponse;
            try
            {
                loginResponse = await _identityApi.LoginAsync(request, cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _log?.FatalException("Error occurred during the login", ex);
                return null;
            }

            try
            {
                var avatarPath = GetAvatarPath(loginResponse);

                await _identityRepository.SetUserInfoAsync(
                        loginResponse.UserId,
                        loginResponse.UserEmail,
                        loginResponse.UserName,
                        request.Password,
                        loginResponse.JwtToken.Token,
                        loginResponse.JwtToken.ExpirationDate,
                        avatarPath,
                        ExternalLoginProvider.None)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _log?.ErrorException("An error occurred when saving login data", ex);
            }

            return loginResponse;
        }

        public async Task<UserLoginResponse> PerformSocialLoginSignInAsync(ExternalLoginProviderRequest request, CancellationToken cancellationToken = default)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            UserLoginResponse loginResponse;
            try
            {
                loginResponse = await _identityApi.SocialLoginAsync(request, cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _log?.FatalException("Error occurred during the social login", ex);
                return null;
            }

            try
            {
                var avatarPath = GetAvatarPath(loginResponse);

                await _identityRepository.SetUserInfoAsync(
                        loginResponse.UserId,
                        loginResponse.UserEmail,
                        loginResponse.UserName,
                        request.UserToken,
                        loginResponse.JwtToken.Token,
                        loginResponse.JwtToken.ExpirationDate,
                        avatarPath,
                        request.SocialProvider)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _log?.ErrorException("Error occurred when saving social login data", ex);
            }

            return loginResponse;
        }

        public async Task RequestTemporaryTokenAsync(ForgotPasswordRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                await _identityApi.RequestTemporaryTokenAsync(request, cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _log?.FatalException("Error requesting temporary token", ex);
                throw;
            }
        }

        public async Task<VerifyUserEmailResponse> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                VerifyUserEmailResponse resetPasswordResponse = await _identityApi.ResetPasswordAsync(request, cancellationToken)
                    .ConfigureAwait(false);

                return resetPasswordResponse;
            }
            catch (Exception ex)
            {
                _log?.FatalException("Error resseting user password", ex);
                return null;
            }
        }

        public async Task<UserSignUpResponse> PerformSignUpAsync(UserSignUpRequest request, StreamPart streamPart, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                UserSignUpResponse response = await _identityApi.SignUpAsync(request.UserName, request.Email, request.Password, streamPart, cancellationToken)
                    .ConfigureAwait(false);

                return response;
            }
            catch (HttpRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _log?.FatalException("An error occurred during the sign up", ex);
                return null;
            }
        }

        public async Task<VerifyUserEmailResponse> ConfirmUserEmailAsync(string userId, string code)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException(nameof(userId));

            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentNullException(nameof(code));

            try
            {
                var request = new EmailConfirmationRequest
                {
                    Code = code,
                    UserId = Guid.Parse(userId)
                };

                return await _identityApi.ConfirmEmailAsync(request)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _log?.FatalException("An error occurred during the email confirmation", ex, userId);
                return null;
            }
        }

        public void PerformLogout()
        {
            try
            {
                _identityRepository.EraseUserInfo();
            }
            catch (Exception ex)
            {
                _log?.ErrorException("An error occurred when logging out", ex);
                throw;
            }
        }

        private static string GetAvatarPath(UserLoginResponse loginResponse) =>
            string.IsNullOrWhiteSpace(loginResponse.AvatarPath) ? "ic_logo.svg" : loginResponse.AvatarPath;
    }

    public interface IIdentityService
    {
        /// <summary>
        /// Get logged user information
        /// </summary>
        /// <returns>User info</returns>
        Task<User> GetLoggedUserAsync();

        /// <summary>
        /// Get logged user token
        /// </summary>
        /// <returns>User token</returns>
        Task<string> GetUserTokenAsync();

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
    }
}
