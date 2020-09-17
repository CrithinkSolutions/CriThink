using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.AI.TextAnalytics;
using CriThink.Server.Providers.NewsAnalyzer.Singletons;
using Microsoft.Extensions.Logging;
using SmartReader;

namespace CriThink.Server.Providers.NewsAnalyzer.Managers
{
    internal class NewsScraperManager : INewsScraperManager
    {
        private readonly ILogger<NewsScraperManager> _logger;

        public NewsScraperManager(ILogger<NewsScraperManager> logger)
        {
            _logger = logger;
        }

        public async Task<NewsScraperProviderResponse> ScrapeNewsWebPage(Uri uri)
        {
            Article article;

            try
            {
                var reader = new Reader(uri.AbsoluteUri);
                article = await reader.GetArticleAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error while scraping the news");
                return null;
            }

            if (!article.IsReadable)
                throw new InvalidOperationException("The given URL doesn't represent a news");

            return new NewsScraperProviderResponse(article, uri);
        }

        public async Task<IReadOnlyList<string>> GetKeywordsFromNewsAsync(NewsScraperProviderResponse scrapedNews)
        {
            if (scrapedNews == null)
                throw new ArgumentNullException(nameof(scrapedNews));

            try
            {
                //var keys = await NewsAnalyticsClient.Instance
                //    .ExtractKeyPhrasesAsync(scrapedNews.NewsBody, "it").ConfigureAwait(false);

                var entities = await NewsAnalyticsClient.Instance
                    .RecognizeEntitiesAsync(scrapedNews.NewsBody, "it").ConfigureAwait(false);

                return entities.Value
                    .Where(e => e.Category == EntityCategory.Location ||
                                e.Category == EntityCategory.Person ||
                                e.Category == EntityCategory.Product ||
                                e.Category == EntityCategory.Organization &&
                                e.ConfidenceScore > 0.20)
                    .Select(c => c.Text)
                    .ToList()
                    .AsReadOnly();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Error getting keywords from news: {scrapedNews.NewsBody}");
                return new List<string>();
            }
        }
    }

    public interface INewsScraperManager
    {
        /// <summary>
        /// Scrape the given news getting its infos
        /// </summary>
        /// <param name="uri">News uri</param>
        /// <returns>News scrape result</returns>
        Task<NewsScraperProviderResponse> ScrapeNewsWebPage(Uri uri);

        /// <summary>
        /// Get the keywords of the given news
        /// </summary>
        /// <param name="scrapedNews">Scraped news</param>
        /// <returns>Keywords collection</returns>
        Task<IReadOnlyList<string>> GetKeywordsFromNewsAsync(NewsScraperProviderResponse scrapedNews);
    }
}
