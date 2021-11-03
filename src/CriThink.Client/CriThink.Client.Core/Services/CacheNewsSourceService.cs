using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Constants;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using CriThink.Common.Helpers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace CriThink.Client.Core.Services
{
    public class CacheNewsSourceService : INewsSourceService, IDisposable
    {
        private const string QuestionsNewsSourceCacheKey = "question_news_source_{0}";

        private readonly IGeolocationService _geoService;
        private readonly IMemoryCache _memoryCache;
        private readonly NewsSourceService _newsSourceService;
        private readonly CancellationTokenSource _resetCacheToken;

        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(15);

        private bool _isDisposed;

        public CacheNewsSourceService(
            IMemoryCache memoryCache,
            IGeolocationService geoService,
            NewsSourceService newsSourceService)
        {
            _geoService = geoService ??
                throw new ArgumentNullException(nameof(geoService));

            _memoryCache = memoryCache ??
                throw new ArgumentNullException(nameof(memoryCache));

            _newsSourceService = newsSourceService ??
                throw new ArgumentNullException(nameof(newsSourceService));

            _resetCacheToken = new CancellationTokenSource();
        }

        public Task RegisterForNotificationAsync(string newsLink, CancellationToken cancellationToken) =>
            _newsSourceService.RegisterForNotificationAsync(newsLink, cancellationToken);

        public async Task<IList<NewsSourceGetQuestionResponse>> GetQuestionsNewsAsync(CancellationToken cancellationToken = default)
        {
            var countryCode = await _geoService.GetCurrentCountryCodeAsync();
            return await _memoryCache.GetOrCreateAsync(QuestionsNewsSourceCacheKey.FormatMe(countryCode.Coalesce(GeoConstant.DEFAULT_LANGUAGE)), async entry =>
            {
                entry.SlidingExpiration = CacheDuration;
                entry.AddExpirationToken(new CancellationChangeToken(_resetCacheToken.Token));
                return await _newsSourceService.GetQuestionsNewsAsync(cancellationToken).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        public Task<NewsSourcePostAnswersResponse> PostAnswersToArticleQuestionsAsync(string newsLink, IList<NewsSourcePostAnswerRequest> questions, CancellationToken cancellationToken)
            => _newsSourceService.PostAnswersToArticleQuestionsAsync(newsLink, questions, cancellationToken);

        public Task UnregisterForNotificationAsync(string newsLink, CancellationToken cancellationToken) =>
            _newsSourceService.UnregisterForNotificationAsync(newsLink, cancellationToken);

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
                _resetCacheToken?.Dispose();
            }

            _isDisposed = true;
        }

        #endregion
    }
}
