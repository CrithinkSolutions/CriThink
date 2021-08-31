using System.Collections.Generic;
using System.Threading.Tasks;

namespace CriThink.Server.Providers.NewsAnalyzer.Services
{
    public interface ITextAnalyticsService
    {
        /// <summary>
        /// Get the keywords of the given news
        /// </summary>
        /// <param name="scrapedNews">Scraped news</param>
        /// <param name="language">Text language</param>
        /// <returns>Keywords collection</returns>
        Task<IReadOnlyList<string>> GetKeywordsFromTextAsync(string text, string language);
    }
}
