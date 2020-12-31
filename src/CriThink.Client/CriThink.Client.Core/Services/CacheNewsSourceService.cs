using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using Microsoft.Extensions.Caching.Memory;

namespace CriThink.Client.Core.Services
{
    public class CacheNewsSourceService : INewsSourceService
    {
        private const string NewsSourceCacheKey = "news_source";

        private readonly IMemoryCache _memoryCache;
        private readonly NewsSourceService _newsSourceService;

        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(15);

        public CacheNewsSourceService(IMemoryCache memoryCache, NewsSourceService newsSourceService)
        {
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            _newsSourceService = newsSourceService ?? throw new ArgumentNullException(nameof(newsSourceService));
        }

        public async Task<NewsSourceSearchResponse> SearchNewsSourceAsync(Uri uri, CancellationToken cancellationToken)
        {
            return await _memoryCache.GetOrCreateAsync(NewsSourceCacheKey, async entry =>
            {
                entry.SlidingExpiration = CacheDuration;
                return await _newsSourceService.SearchNewsSourceAsync(uri, cancellationToken).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }
    }
}
