using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SmartReader;

namespace CriThink.Server.Providers.NewsAnalyzer.Services
{
    internal class NewsScraperService : INewsScraperService
    {
        private readonly ILogger<NewsScraperService> _logger;

        public NewsScraperService(
            ILogger<NewsScraperService> logger)
        {
            _logger = logger;
        }

        public async Task<NewsScraperProviderResponse> ScrapeNewsWebPage(Uri uri)
        {
            Article article;

            try
            {
                var reader = new Reader(uri.AbsoluteUri);
                article = await reader.GetArticleAsync();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error while scraping the news");
                return null;
            }

            if (!article.IsReadable)
            {
                _logger?.LogWarning("The given linkdoes not represent a news", uri.AbsoluteUri);
                throw new InvalidOperationException("The given URL doesn't represent a news");
            }

            return new NewsScraperProviderResponse(article, uri);
        }
    }
}
