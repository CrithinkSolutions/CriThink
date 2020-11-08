﻿using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Models.Identity;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using Microsoft.Extensions.Caching.Memory;

namespace CriThink.Client.Core.Services
{
    public class CacheIdentityService : IIdentityService
    {
        private const string UserCacheKey = "current_user";

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

        public Task<UserLoginResponse> PerformLoginAsync(UserLoginRequest request, CancellationToken cancellationToken) =>
            _identityService.PerformLoginAsync(request, cancellationToken);

        public Task<UserLoginResponse> PerformSocialLoginSignInAsync(ExternalLoginProviderRequest request, CancellationToken cancellationToken = default) =>
            _identityService.PerformSocialLoginSignInAsync(request, cancellationToken);

        public Task RequestTemporaryTokenAsync(ForgotPasswordRequest request, CancellationToken cancellationToken) =>
            _identityService.RequestTemporaryTokenAsync(request, cancellationToken);

        public Task<VerifyUserEmailResponse> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken cancellationToken) =>
            _identityService.ResetPasswordAsync(request, cancellationToken);

        public Task<UserSignUpResponse> PerformSignUpAsync(UserSignUpRequest request, CancellationToken cancellationToken) =>
            _identityService.PerformSignUpAsync(request, cancellationToken);

    }
}