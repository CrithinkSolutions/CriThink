using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Messenger;
using CriThink.Client.Core.Models.Identity;
using CriThink.Common.Endpoints.DTOs.UserProfile;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using MvvmCross.Plugin.Messenger;
using MvvmCross.WeakSubscription;
using Refit;

namespace CriThink.Client.Core.Services
{
    public class CacheUserProfileService : IUserProfileService, IDisposable
    {
        private const string UserProfileKey = "user_profile";
        private const string RecentNewsSourceCacheKey = "recent_news_source";

        private readonly IMemoryCache _memoryCache;
        private readonly UserProfileService _userProfileService;
        private readonly MvxSubscriptionToken _tokenLogoutMessage;
        private readonly MvxSubscriptionToken _tokenClearSearchesMessage;

        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

        private CancellationTokenSource _resetCacheToken;
        private bool _isDisposed;

        public CacheUserProfileService(
            IMemoryCache memoryCache,
            UserProfileService userProfileService,
            IMvxMessenger messenger)
        {
            _memoryCache = memoryCache ??
                throw new ArgumentNullException(nameof(memoryCache));

            _userProfileService = userProfileService ??
                throw new ArgumentNullException(nameof(userProfileService));

            _tokenLogoutMessage = messenger?.Subscribe<LogoutPerformedMessage>(OnLogoutPerformed) ??
                throw new ArgumentNullException(nameof(messenger));

            _tokenClearSearchesMessage = messenger?.Subscribe<ClearRecentNewsSourceCacheMessage>(OnClearRecentNewsSourceCache, reference: MvxReference.Weak) ??
                throw new ArgumentNullException(nameof(messenger));

            _resetCacheToken = new CancellationTokenSource();
        }

        public async Task<User> GetUserProfileAsync(CancellationToken cancellationToken = default)
        {
            return await _memoryCache.GetOrCreateAsync($"{UserProfileKey}", async entry =>
            {
                entry.SlidingExpiration = CacheDuration;
                return await _userProfileService.GetUserProfileAsync(cancellationToken).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        public async Task UpdateUserProfileAsync(UserProfileUpdateRequest request, CancellationToken cancellationToken = default)
        {
            await _userProfileService.UpdateUserProfileAsync(request, cancellationToken).ConfigureAwait(false);
            ClearUserInfoFromCache();
        }

        public async Task UpdateUserProfileAvatarAsync(StreamPart streamPart, CancellationToken cancellationToken = default)
        {
            await _userProfileService.UpdateUserProfileAvatarAsync(streamPart, cancellationToken).ConfigureAwait(false);
            ClearUserInfoFromCache();
        }

        public async Task<UserProfileGetAllRecentSearchResponse> GetUserRecentSearchesAsync(CancellationToken cancellationToken = default)
        {
            return await _memoryCache.GetOrCreateAsync(RecentNewsSourceCacheKey, async entry =>
            {
                entry.SlidingExpiration = CacheDuration;
                entry.AddExpirationToken(new CancellationChangeToken(_resetCacheToken.Token));
                return await _userProfileService.GetUserRecentSearchesAsync().ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        private void ClearUserInfoFromCache()
        {
            _memoryCache.Remove(UserProfileKey);
        }

        private void OnLogoutPerformed(LogoutPerformedMessage message)
        {
            ClearUserInfoFromCache();
        }

        private void OnClearRecentNewsSourceCache(ClearRecentNewsSourceCacheMessage message)
        {
            if (_resetCacheToken != null && !_resetCacheToken.IsCancellationRequested && _resetCacheToken.Token.CanBeCanceled)
            {
                _resetCacheToken.Cancel();
                _resetCacheToken.Dispose();
            }

            _resetCacheToken = new CancellationTokenSource();
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            if (disposing)
            {
                _memoryCache?.Dispose();
                _tokenLogoutMessage?.Dispose();
                _tokenClearSearchesMessage?.Dispose();
                _resetCacheToken?.Dispose();
            }

            _isDisposed = true;
        }

        #endregion
    }
}