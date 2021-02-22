using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Messenger;
using CriThink.Client.Core.Models.NewsChecker;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using MvvmCross.Plugin.Messenger;

namespace CriThink.Client.Core.Services
{
    public class CacheNewsSourceService : INewsSourceService, IDisposable
    {
        private const string NewsSourceCacheKey = "news_source";
        private const string RecentNewsSourceCacheKey = "recent_news_source";

        private readonly IMemoryCache _memoryCache;
        private readonly NewsSourceService _newsSourceService;
        private readonly MvxSubscriptionToken _token;

        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(15);

        private CancellationTokenSource _resetCacheToken;
        private bool _isDisposed;

        public CacheNewsSourceService(IMemoryCache memoryCache, NewsSourceService newsSourceService, IMvxMessenger messenger)
        {
            _resetCacheToken = new CancellationTokenSource();
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            _newsSourceService = newsSourceService ?? throw new ArgumentNullException(nameof(newsSourceService));
            _token = messenger?.Subscribe<ClearRecentNewsSourceCacheMessage>(OnClearRecentNewsSourceCache) ?? throw new ArgumentNullException(nameof(messenger));
        }

        public async Task<NewsSourceSearchWithDebunkingNewsResponse> SearchNewsSourceAsync(Uri uri, CancellationToken cancellationToken)
        {
            return await _memoryCache.GetOrCreateAsync($"{NewsSourceCacheKey}_{uri}", async entry =>
            {
                entry.SlidingExpiration = CacheDuration;
                return await _newsSourceService.SearchNewsSourceAsync(uri, cancellationToken).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        public async Task<IList<RecentNewsChecksModel>> GetLatestNewsChecks()
        {
            return await _memoryCache.GetOrCreateAsync(RecentNewsSourceCacheKey, async entry =>
            {
                entry.SlidingExpiration = CacheDuration;
                entry.AddExpirationToken(new CancellationChangeToken(_resetCacheToken.Token));
                return await _newsSourceService.GetLatestNewsChecks().ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        public Task RegisterForNotificationAsync(Uri uri, CancellationToken cancellationToken) =>
            _newsSourceService.RegisterForNotificationAsync(uri, cancellationToken);

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
                _token?.Dispose();
                _resetCacheToken?.Dispose();
            }

            _isDisposed = true;
        }

        #endregion
    }
}
