using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CriThink.Server.Providers.NewsAnalyzer.Managers
{
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