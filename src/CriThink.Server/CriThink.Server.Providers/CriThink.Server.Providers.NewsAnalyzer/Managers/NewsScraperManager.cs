using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.AI.TextAnalytics;
using CriThink.Common.Helpers;
using CriThink.Server.Providers.NewsAnalyzer.Singletons;
using Microsoft.Extensions.Logging;
using SmartReader;

namespace CriThink.Server.Providers.NewsAnalyzer.Managers
{
    internal class NewsScraperManager : INewsScraperManager
    {
        private readonly NewsAnalyticsClient _newsAnalyticsClient;
        private readonly ILogger<NewsScraperManager> _logger;

        public NewsScraperManager(NewsAnalyticsClient newsAnalyticsClient, ILogger<NewsScraperManager> logger)
        {
            _newsAnalyticsClient = newsAnalyticsClient ?? throw new ArgumentNullException(nameof(newsAnalyticsClient));
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

            var allKeywords = new List<string>();

            var bodyParts = scrapedNews.NewsBody.SplitInParts(5120).ToList(); // max lenght supported by Azure

            foreach (var bodyPart in bodyParts)
            {
                try
                {
                    var keywords = await SendAnalysisRequestAsync(bodyPart.ToString()).ConfigureAwait(false);
                    allKeywords.AddRange(keywords);
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, $"Error getting keywords from news '{scrapedNews.NewsBody}': {bodyPart}");
                }
            }

            return allKeywords
                .Distinct()
                .ToList()
                .AsReadOnly();
        }

        private async Task<IReadOnlyList<string>> SendAnalysisRequestAsync(string newsBody, string language = "it")
        {
            var entities = await _newsAnalyticsClient.Instance.RecognizeEntitiesAsync(newsBody, language)
                .ConfigureAwait(false);

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
    }
}
