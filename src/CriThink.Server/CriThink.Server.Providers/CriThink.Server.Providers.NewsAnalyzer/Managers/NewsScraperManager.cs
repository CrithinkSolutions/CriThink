using System;
using System.Threading.Tasks;
using CriThink.Server.Providers.NewsAnalyzer.Responses;
using SmartReader;

namespace CriThink.Server.Providers.NewsAnalyzer.Managers
{
    internal class NewsScraperManager : INewsScraperManager
    {
        public async Task<NewsScraperResponse> ScrapeNewsWebPage(Uri uri)
        {
            var reader = new Reader(uri.AbsoluteUri);

            var article = await reader.GetArticleAsync().ConfigureAwait(false);

            if (!article.IsReadable)
                throw new InvalidOperationException("The given URL doesn't represent a news");

            return new NewsScraperResponse(article);
        }
    }

    public interface INewsScraperManager
    {
        /// <summary>
        /// Scrape the given news getting its infos
        /// </summary>
        /// <param name="uri">News uri</param>
        /// <returns>News scrape result</returns>
        Task<NewsScraperResponse> ScrapeNewsWebPage(Uri uri);
    }
}
