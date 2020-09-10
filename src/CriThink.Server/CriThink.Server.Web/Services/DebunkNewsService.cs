using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CriThink.Server.Providers.NewsAnalyzer;
using CriThink.Server.Providers.NewsAnalyzer.Managers;
using CriThink.Server.Web.Facades;

namespace CriThink.Server.Web.Services
{
    public class DebunkNewsService : IDebunkNewsService
    {
        private readonly IDebunkNewsFetcherFacade _debunkNewsFetcherFacade;
        private readonly INewsScraperManager _newsScraperManager;

        public DebunkNewsService(IDebunkNewsFetcherFacade debunkNewsFetcherFacade, INewsScraperManager newsScraperManager)
        {
            _debunkNewsFetcherFacade = debunkNewsFetcherFacade ?? throw new ArgumentNullException(nameof(debunkNewsFetcherFacade));
            _newsScraperManager = newsScraperManager ?? throw new ArgumentNullException(nameof(newsScraperManager));
        }

        public async Task UpdateRepositoryAsync()
        {
            var debunkingNewsCollection = await _debunkNewsFetcherFacade.FetchOpenOnlineDebunkNewsAsync().ConfigureAwait(false);

            var scrapedNewsCollection = new List<NewsScraperProviderResponse>();

            foreach (var debunkingNews in debunkingNewsCollection)
            {
                if (debunkingNews.HasError)
                    throw debunkingNews.Exception;

                foreach (var response in debunkingNews.Responses)
                {
                    var scrapedNews = await _newsScraperManager.ScrapeNewsWebPage(new Uri(response.Link)).ConfigureAwait(false);
                    scrapedNewsCollection.Add(scrapedNews);
                }
            }

            // TODO: Add in db
        }
    }

    public interface IDebunkNewsService
    {
        /// <summary>
        /// Update the debunk news repository
        /// </summary>
        /// <returns></returns>
        Task UpdateRepositoryAsync();
    }
}
