using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Messenger;
using CriThink.Common.Endpoints.DTOs.Admin;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using MvvmCross.Plugin.Messenger;

namespace CriThink.Client.Core.Services
{
    public class CacheDebunkingNewsService : IDebunkingNewsService, IDisposable
    {
        private const string AllDebunkingNewsCacheKey = "all_debunking";
        private const string DebunkingNewsCacheKey = "debunking";

        private CancellationTokenSource _resetCacheToken;

        private readonly IMemoryCache _memoryCache;
        private readonly DebunkingNewsService _debunkingNewsService;
        private readonly MvxSubscriptionToken _token;

        private bool _isDisposed;

        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

        public CacheDebunkingNewsService(IMemoryCache memoryCache, DebunkingNewsService debunkingNewsService, IMvxMessenger messenger)
        {
            _resetCacheToken = new CancellationTokenSource();
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            _debunkingNewsService = debunkingNewsService ?? throw new ArgumentNullException(nameof(debunkingNewsService));
            _token = messenger?.Subscribe<ClearDebunkingNewsCacheMessage>(OnClearMessage) ?? throw new ArgumentNullException(nameof(messenger));
        }

        public async Task<DebunkingNewsGetAllResponse> GetRecentDebunkingNewsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            return await _memoryCache.GetOrCreateAsync($"{AllDebunkingNewsCacheKey}_{pageIndex}_{pageSize}", async entry =>
            {
                entry.SlidingExpiration = CacheDuration;
                entry.AddExpirationToken(new CancellationChangeToken(_resetCacheToken.Token));
                return await _debunkingNewsService.GetRecentDebunkingNewsAsync(pageIndex, pageSize, cancellationToken).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        public async Task<DebunkingNewsGetDetailsResponse> GetDebunkingNewsByIdAsync(string id, CancellationToken cancellationToken)
        {
            return await _memoryCache.GetOrCreateAsync($"{DebunkingNewsCacheKey}_{id}", async entry =>
            {
                entry.SlidingExpiration = CacheDuration;
                return await _debunkingNewsService.GetDebunkingNewsByIdAsync(id, cancellationToken).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        private void OnClearMessage(ClearDebunkingNewsCacheMessage obj)
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
