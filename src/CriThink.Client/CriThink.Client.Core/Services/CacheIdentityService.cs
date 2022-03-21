using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Messenger;
using CriThink.Client.Core.Models.Identity;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using Microsoft.Extensions.Caching.Memory;
using MvvmCross.Plugin.Messenger;
using Refit;

namespace CriThink.Client.Core.Services
{
    public class CacheIdentityService : IIdentityService
    {
        private const string UserTokenCacheKey = "current_user_token";
        private const string UsernameAvailabilityCacheKey = "username_availability";

        private readonly IdentityService _identityService;
        private readonly IMemoryCache _memoryCache;
        private readonly IMvxMessenger _messenger;

        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

        public CacheIdentityService(
            IMemoryCache memoryCache,
            IdentityService identityService,
            IMvxMessenger messenger)
        {
            _memoryCache = memoryCache ??
                throw new ArgumentNullException(nameof(memoryCache));

            _identityService = identityService ??
                throw new ArgumentNullException(nameof(identityService));

            _messenger = messenger ??
                throw new ArgumentNullException(nameof(messenger));
        }

        public async Task<UserAccess> GetLoggedUserAccessAsync()
        {
            return await _memoryCache.GetOrCreateAsync(UserTokenCacheKey, async entry =>
            {
                entry.SlidingExpiration = CacheDuration;
                return await _identityService.GetLoggedUserAccessAsync().ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        public async Task<UserLoginResponse> PerformLoginAsync(UserLoginRequest request, CancellationToken cancellationToken)
        {
            var response = await _identityService.PerformLoginAsync(request, cancellationToken).ConfigureAwait(false);
            ClearUserInfoFromCache();
            return response;
        }

        public async Task PerformSocialLoginSignInAsync(ExternalLoginProvider loginProvider)
        {
            await _identityService.PerformSocialLoginSignInAsync(loginProvider)
                .ConfigureAwait(false);

            ClearUserInfoFromCache();
        }

        public async Task<UserRefreshTokenResponse> ExchangeTokensAsync(CancellationToken cancellationToken = default)
        {
            var response = await _identityService.ExchangeTokensAsync(cancellationToken);
            ClearUserInfoFromCache();
            return response;
        }

        public async Task RequestTemporaryTokenAsync(ForgotPasswordRequest request, CancellationToken cancellationToken)
        {
            await _identityService.RequestTemporaryTokenAsync(request, cancellationToken).ConfigureAwait(false);
            ClearUserInfoFromCache();
        }

        public async Task<VerifyUserEmailResponse> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken cancellationToken)
        {
            var response = await _identityService.ResetPasswordAsync(request, cancellationToken).ConfigureAwait(false);
            ClearUserInfoFromCache();
            return response;
        }

        public async Task<UserSignUpResponse> PerformSignUpAsync(UserSignUpRequest request, StreamPart stream, CancellationToken cancellationToken)
        {
            var response = await _identityService.PerformSignUpAsync(request, stream, cancellationToken).ConfigureAwait(false);
            ClearUserInfoFromCache();
            return response;
        }

        public async Task<VerifyUserEmailResponse> ConfirmUserEmailAsync(string userId, string code)
        {
            var response = await _identityService.ConfirmUserEmailAsync(userId, code).ConfigureAwait(false);
            ClearUserInfoFromCache();
            return response;
        }

        public void PerformLogout()
        {
            try
            {
                _identityService.PerformLogout();
            }
            finally
            {
                ClearUserInfoFromCache();
            }
        }

        public async Task<UserSoftDeletionResponse> DeleteAccountAsync(CancellationToken cancellationToken)
        {
            var response = await _identityService.DeleteAccountAsync(cancellationToken);
            ClearUserInfoFromCache();
            return response;
        }

        public async Task RestoreDeletedAccountAsync(RestoreUserRequest request, CancellationToken cancellationToken)
        {
            await _identityService.RestoreDeletedAccountAsync(request, cancellationToken);
            ClearUserInfoFromCache();
        }

        public async Task<bool> CheckForUsernameAvailabilityAsync(string username, CancellationToken cancellationToken = default)
        {
            return await _memoryCache.GetOrCreateAsync($"{UsernameAvailabilityCacheKey}_{username}", async entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromMinutes(1);
                return await _identityService.CheckForUsernameAvailabilityAsync(username, cancellationToken);
            }).ConfigureAwait(false);
        }

        private void ClearUserInfoFromCache()
        {
            _memoryCache.Remove(UserTokenCacheKey);

            var message = new LogoutPerformedMessage(this);
            _messenger.Publish(message);
        }
    }
}
