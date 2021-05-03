using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Models.Identity;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using Microsoft.Extensions.Caching.Memory;
using Refit;

namespace CriThink.Client.Core.Services
{
    public class CacheIdentityService : IIdentityService
    {
        private const string UserCacheKey = "current_user";
        private const string UserTokenCacheKey = "current_user_token";

        private readonly IdentityService _identityService;
        private readonly IMemoryCache _memoryCache;

        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

        public CacheIdentityService(IMemoryCache memoryCache, IdentityService identityService)
        {
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }

        public async Task<User> GetLoggedUserAsync()
        {
            return await _memoryCache.GetOrCreateAsync(UserCacheKey, async entry =>
            {
                entry.SlidingExpiration = CacheDuration;
                return await _identityService.GetLoggedUserAsync().ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        public async Task<UserLoginResponse> PerformLoginAsync(UserLoginRequest request, CancellationToken cancellationToken)
        {
            var response = await _identityService.PerformLoginAsync(request, cancellationToken).ConfigureAwait(false);
            ClearUserInfoFromCache();
            return response;
        }

        public async Task<UserLoginResponse> PerformSocialLoginSignInAsync(ExternalLoginProviderRequest request, CancellationToken cancellationToken = default)
        {
            var response = await _identityService.PerformSocialLoginSignInAsync(request, cancellationToken).ConfigureAwait(false);
            ClearUserInfoFromCache();
            return response;
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

        private void ClearUserInfoFromCache()
        {
            _memoryCache.Remove(UserCacheKey);
            _memoryCache.Remove(UserTokenCacheKey);
        }
    }
}
