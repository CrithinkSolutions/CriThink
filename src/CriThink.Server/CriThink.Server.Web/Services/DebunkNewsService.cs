using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CriThink.Server.Core.Commands;
using CriThink.Server.Core.Entities;
using CriThink.Server.Providers.NewsAnalyzer;
using CriThink.Server.Providers.NewsAnalyzer.Managers;
using CriThink.Server.Web.Facades;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Web.Services
{
    public class DebunkNewsService : IDebunkNewsService
    {
        private readonly IDebunkNewsFetcherFacade _debunkNewsFetcherFacade;
        private readonly INewsScraperManager _newsScraperManager;
        private readonly IMediator _mediator;
        private readonly ILogger<DebunkNewsService> _logger;

        public DebunkNewsService(IDebunkNewsFetcherFacade debunkNewsFetcherFacade, INewsScraperManager newsScraperManager, IMediator mediator, ILogger<DebunkNewsService> logger)
        {
            _debunkNewsFetcherFacade = debunkNewsFetcherFacade ?? throw new ArgumentNullException(nameof(debunkNewsFetcherFacade));
            _newsScraperManager = newsScraperManager ?? throw new ArgumentNullException(nameof(newsScraperManager));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger;
        }

        public async Task UpdateRepositoryAsync()
        {
            var debunkingNewsCollection = await _debunkNewsFetcherFacade.FetchOpenOnlineDebunkNewsAsync().ConfigureAwait(false);

            var debunkedNewsCollection = new List<DebunkingNews>();

            foreach (var debunkingNews in debunkingNewsCollection)
            {
                if (debunkingNews.HasError)
                    throw debunkingNews.Exception;

                foreach (var response in debunkingNews.Responses)
                {
                    try
                    {
                        var scrapedNews = await _newsScraperManager.ScrapeNewsWebPage(new Uri(response.Link))
                            .ConfigureAwait(false);

                        var entity = await GetNewsKeywordsAsync(scrapedNews)
                            .ConfigureAwait(false);

                        debunkedNewsCollection.Add(entity);
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, $"Error scraping news to update the repository: '{response.Link}'");
                    }
                }
            }

            if (!debunkedNewsCollection.Any())
                return;

            var command = new CreateDebunkingNewsCommand(debunkedNewsCollection);
            var _ = await _mediator.Send(command).ConfigureAwait(false);
        }

        private async Task<DebunkingNews> GetNewsKeywordsAsync(NewsScraperProviderResponse scrapedNews)
        {
            var keywords = await _newsScraperManager.GetKeywordsFromNewsAsync(scrapedNews)
                .ConfigureAwait(false);

            var entity = new DebunkingNews
            {
                Keywords = string.Join(',', keywords),
                Link = scrapedNews.RequestedUri.AbsoluteUri,
                NewsCaption = scrapedNews.GetCaption(),
                PublisherName = scrapedNews.WebSiteName,
                Title = scrapedNews.Title
            };

            if (scrapedNews.Date.HasValue)
                entity.PublishingDate = scrapedNews.Date.Value;

            return entity;
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
