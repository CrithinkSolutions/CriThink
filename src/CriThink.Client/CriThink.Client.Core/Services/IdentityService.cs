using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Api;
using CriThink.Client.Core.Constants;
using CriThink.Client.Core.Exceptions;
using CriThink.Client.Core.Models.Identity;
using CriThink.Client.Core.Repositories;
using CriThink.Common.Endpoints;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using CriThink.Common.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Refit;
using Xamarin.Essentials;

namespace CriThink.Client.Core.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IIdentityApi _identityApi;
        private readonly IConfiguration _configuration;
        private readonly IAuthorizedIdentityApi _authorizedIdentityApi;
        private readonly IIdentityRepository _identityRepository;
        private readonly IGeolocationService _geoService;
        private readonly ILogger<IdentityService> _logger;

        public IdentityService(
            IIdentityApi identityApi,
            IConfiguration configuration,
            IAuthorizedIdentityApi authorizedIdentityApi,
            IIdentityRepository identityRepository,
            IGeolocationService geoService,
            ILogger<IdentityService> logger)
        {
            _identityApi = identityApi ??
                throw new ArgumentNullException(nameof(identityApi));

            _configuration = configuration ??
                throw new ArgumentNullException(nameof(configuration));

            _authorizedIdentityApi = authorizedIdentityApi ??
                throw new ArgumentNullException(nameof(authorizedIdentityApi));

            _identityRepository = identityRepository ??
                throw new ArgumentNullException(nameof(identityRepository));

            _geoService = geoService ??
                throw new ArgumentNullException(nameof(geoService));

            _logger = logger;
        }

        public Task<UserAccess> GetLoggedUserAccessAsync()
        {
            try
            {
                return _identityRepository.GetUserAccessAsync();
            }
            catch (Exception ex)
            {
                _logger?.LogCritical(ex, "Can't get user access");
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
                _logger?.LogCritical(ex, "Error occurred during the login");
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
                _logger?.LogError(ex, "An error occurred when saving login data");
            }

            return loginResponse;
        }

        public async Task PerformSocialLoginSignInAsync(
            ExternalLoginProvider loginProvider)
        {
            string refreshToken;
            string accessToken;
            DateTime tokenExpirationTime;

            try
            {
                var endpoint = _configuration["BaseApiUri"];
                var authUrl = new Uri($"{endpoint}identity/external-login/{loginProvider}");
                var callbackUrl = new Uri(EndpointConstants.AppSchema);

                WebAuthenticatorResult result = await WebAuthenticator.AuthenticateAsync(authUrl, callbackUrl)
                    .ConfigureAwait(true);

                var customProperties = result.Properties;

                refreshToken = customProperties["refresh_token"];
                accessToken = customProperties["access_token"];
                var accessExpiresInTicks = customProperties["jwt_token_expires"];
                tokenExpirationTime = new DateTime(long.Parse(accessExpiresInTicks));
            }
            catch (Exception ex)
            {
                _logger?.LogCritical(ex, "Error occurred during the social login");
                throw;
            }

            try
            {
                var jwtToken = new JwtTokenResponse
                {
                    Token = accessToken,
                    ExpirationDate = tokenExpirationTime
                };

                var userAccess = new UserAccess(jwtToken, refreshToken);

                await _identityRepository.SetUserAccessAsync(userAccess)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error occurred when saving social login data");
                throw;
            }
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
                _logger?.LogCritical(ex, "Error requesting temporary token");
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
                _logger?.LogCritical(ex, "Error resseting user password");
                return null;
            }
        }

        public async Task<UserSignUpResponse> PerformSignUpAsync(UserSignUpRequest request, StreamPart streamPart, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                var language = await _geoService.GetCurrentCountryCodeAsync();
                if (string.IsNullOrWhiteSpace(language))
                    language = GeoConstant.DEFAULT_LANGUAGE;

                UserSignUpResponse response = await _identityApi.SignUpAsync(
                    request.Username,
                    request.Email,
                    request.Password,
                    streamPart,
                    language,
                    cancellationToken)
                    .ConfigureAwait(false);

                return response;
            }
            catch (ApiException ex) when (ex.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new CriThinkSignUpException(ex.Content);
            }
            catch (Exception ex)
            {
                _logger?.LogCritical(ex, "An error occurred during the sign up");
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
                _logger?.LogCritical(ex, "An error occurred during the email confirmation", userId);
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
                _logger?.LogError(ex, "An error occurred when logging out");
                throw;
            }
        }

        public async Task<UserSoftDeletionResponse> DeleteAccountAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await _authorizedIdentityApi.DeleteUserAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "An error occurred when deleting the account");
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
                _logger?.LogError(ex, "An error occurred when restoring a deleted account");
                throw;
            }
        }

        public async Task<bool> CheckForUsernameAvailabilityAsync(string username, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(username))
                return false;

            var request = new UsernameAvailabilityRequest
            {
                Username = username,
            };

            try
            {
                var result = await _identityApi.GetUsernameAvailabilityAsync(request, cancellationToken);
                return result.IsAvailable;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "An error occurred when checking for username availability");
                throw;
            }
        }
    }
}
