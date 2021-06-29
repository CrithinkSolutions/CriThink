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

        public Task<UserAccess> GetLoggedUserAccessAsync()
        {
            try
            {
                return _identityRepository.GetUserAccessAsync();
            }
            catch (Exception ex)
            {
                _log?.FatalException("Can't get user access", ex);
                return null;
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
                var userAccess = new UserAccess(loginResponse);

                await _identityRepository.SetUserAccessAsync(userAccess)
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
                var user = new UserAccess(loginResponse);

                await _identityRepository.SetUserAccessAsync(user)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _log?.ErrorException("Error occurred when saving social login data", ex);
            }

            return loginResponse;
        }

        public async Task<UserRefreshTokenResponse> ExchangeTokensAsync(CancellationToken cancellationToken = default)
        {
            var userAccess = await _identityRepository.GetUserAccessAsync();

            var currentAccessToken = userAccess.JwtToken.Token;
            if (string.IsNullOrWhiteSpace(currentAccessToken))
                throw new InvalidOperationException("No valid token found");

            var refreshToken = userAccess.RefreshToken;
            if (string.IsNullOrWhiteSpace(refreshToken))
                throw new InvalidOperationException("No valid refresh token found");

            var request = new UserRefreshTokenRequest
            {
                RefreshToken = refreshToken,
                AccessToken = currentAccessToken,
            };

            var newerTokens = await _identityApi.ExchangeTokensAsync(request, cancellationToken)
                .ConfigureAwait(false);

            userAccess.UpdateJwtTokens(newerTokens);

            await _identityRepository.SetUserAccessAsync(userAccess)
                .ConfigureAwait(false);

            return newerTokens;
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

        public async Task<UserSoftDeletionResponse> DeleteAccountAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await _identityApi.DeleteUserAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _log?.ErrorException("An error occurred when deleting the account", ex);
                throw;
            }
        }

        public async Task RestoreDeletedAccountAsync(RestoreUserRequest request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                await _identityApi.RestoreUserAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                _log?.ErrorException("An error occurred when restoring a deleted account", ex);
                throw;
            }
        }
    }
}
