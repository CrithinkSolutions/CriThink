using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Constants;
using CriThink.Client.Core.Messenger;
using CriThink.Client.Core.Models.NewsChecker;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using CriThink.Common.Helpers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;

namespace CriThink.Client.Core.Services
{
    public class CacheNewsSourceService : INewsSourceService, IDisposable
    {
        private const string RecentNewsSourceCacheKey = "recent_news_source";
        private const string QuestionsNewsSourceCacheKey = "question_news_source_{0}";

        private readonly IGeolocationService _geoService;
        private readonly IMemoryCache _memoryCache;
        private readonly NewsSourceService _newsSourceService;
        private readonly MvxSubscriptionToken _token;

        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(15);

        private CancellationTokenSource _resetCacheToken;
        private bool _isDisposed;

        public CacheNewsSourceService(
            IMemoryCache memoryCache,
            IGeolocationService geoService,
            NewsSourceService newsSourceService,
            IMvxMessenger messenger)
        {
            _geoService = geoService ?? throw new ArgumentNullException(nameof(geoService));
            _resetCacheToken = new CancellationTokenSource();
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            _newsSourceService = newsSourceService ?? throw new ArgumentNullException(nameof(newsSourceService));
            _token = messenger?.Subscribe<ClearRecentNewsSourceCacheMessage>(OnClearRecentNewsSourceCache) ?? throw new ArgumentNullException(nameof(messenger));
        }

        public async Task<IList<RecentNewsChecksModel>> GetLatestNewsChecksAsync(IMvxAsyncCommand<RecentNewsChecksModel> deleteHistoryRecentNewsItemCommand)
        {
            return await _memoryCache.GetOrCreateAsync(RecentNewsSourceCacheKey, async entry =>
            {
                entry.SlidingExpiration = CacheDuration;
                entry.AddExpirationToken(new CancellationChangeToken(_resetCacheToken.Token));
                return await _newsSourceService.GetLatestNewsChecksAsync(deleteHistoryRecentNewsItemCommand).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        public Task RegisterForNotificationAsync(string newsLink, CancellationToken cancellationToken) =>
            _newsSourceService.RegisterForNotificationAsync(newsLink, cancellationToken);


        public async Task<IList<NewsSourceGetQuestionResponse>> GetQuestionsNewsAsync(CancellationToken cancellationToken)
        {
            var currentArea = await _geoService.GetCurrentCountryCodeAsync().ConfigureAwait(false);
            return await _memoryCache.GetOrCreateAsync(QuestionsNewsSourceCacheKey.FormatMe(currentArea.Coalesce(GeoConstant.DEFAULT_LANGUAGE)), async entry =>
            {
                entry.SlidingExpiration = CacheDuration;
                entry.AddExpirationToken(new CancellationChangeToken(_resetCacheToken.Token));
                return await _newsSourceService.GetQuestionsNewsAsync(cancellationToken).ConfigureAwait(false);
            }).ConfigureAwait(false);
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
                _token?.Dispose();
                _resetCacheToken?.Dispose();
            }

            _isDisposed = true;
        }

        public Task<NewsSourcePostAnswersResponse> PostAnswersToArticleQuestionsAsync(string newsLink, IList<NewsSourcePostAnswerRequest> questions, CancellationToken cancellationToken)
            => _newsSourceService.PostAnswersToArticleQuestionsAsync(newsLink, questions, cancellationToken);

        #endregion
    }
}
