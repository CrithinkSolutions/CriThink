using System;
using System.Threading.Tasks;

namespace CriThink.Server.Providers.NewsAnalyzer.Services
{
    public interface INewsScraperService
    {
        /// <summary>
        /// Scrape the given news
        /// </summary>
        /// <param name="uri">News uri</param>
        /// <returns>News scrape result</returns>
        Task<NewsScraperProviderResponse> ScrapeNewsWebPage(Uri uri);
    }
}
