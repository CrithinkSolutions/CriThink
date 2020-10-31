using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.Admin;
using CriThink.Server.Core.Commands;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Queries;
using CriThink.Server.Providers.DebunkNewsFetcher;
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

        public async Task AddDebunkingNewsAsync(DebunkingNewsAddRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var scrapedNews = await _newsScraperManager.ScrapeNewsWebPage(new Uri(request.Link))
                .ConfigureAwait(false);

            var keywords = await GetNewsKeywordsAsync(scrapedNews).ConfigureAwait(false);

            var entity = BuildDebunkingNewsEntity(scrapedNews, keywords, request);

            var command = new CreateDebunkingNewsCommand(new[] { entity });
            var _ = await _mediator.Send(command).ConfigureAwait(false);
        }

        public async Task UpdateRepositoryAsync()
        {
            var query = new GetLastDebunkinNewsFetchTimeQuery();
            var lastDateTask = _mediator.Send(query);
            var debunkinNewsTask = _debunkNewsFetcherFacade.FetchOpenOnlineDebunkNewsAsync();

            await Task.WhenAll(lastDateTask, debunkinNewsTask).ConfigureAwait(false);

            var lastSuccessfullFetchDate = lastDateTask.Result;
            var debunkingNewsCollection = debunkinNewsTask.Result;

            var debunkedNewsCollection = await ScrapeDebunkingNewsCollectionAsync(debunkingNewsCollection, lastSuccessfullFetchDate).ConfigureAwait(false);

            if (!debunkedNewsCollection.Any())
                return;

            var command = new CreateDebunkingNewsCommand(debunkedNewsCollection);
            var _ = await _mediator.Send(command).ConfigureAwait(false);
        }

        private async Task<List<DebunkingNews>> ScrapeDebunkingNewsCollectionAsync(IEnumerable<DebunkingNewsProviderResult> debunkingNewsCollection, DateTime lastSuccessfullFetchDate)
        {
            var debunkedNewsCollection = new List<DebunkingNews>();

            foreach (var debunkingNews in debunkingNewsCollection)
            {
                if (debunkingNews.HasError)
                    throw debunkingNews.Exception;

                foreach (var response in debunkingNews.Responses.Where(r => r.PublishingDate > lastSuccessfullFetchDate))
                {
                    try
                    {
                        var scrapedNews = await ScrapeNewsAsync(response.Link)
                            .ConfigureAwait(false);

                        var keywords = await GetNewsKeywordsAsync(scrapedNews)
                            .ConfigureAwait(false);

                        var entity = BuildDebunkingNewsEntity(scrapedNews, keywords);

                        debunkedNewsCollection.Add(entity);
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, $"Error scraping news to update the repository: '{response.Link}'");
                    }
                }
            }

            return debunkedNewsCollection;
        }

        private Task<NewsScraperProviderResponse> ScrapeNewsAsync(string link) =>
            _newsScraperManager.ScrapeNewsWebPage(new Uri(link));

        private Task<IReadOnlyList<string>> GetNewsKeywordsAsync(NewsScraperProviderResponse scrapedNews) =>
            _newsScraperManager.GetKeywordsFromNewsAsync(scrapedNews);

        private static DebunkingNews BuildDebunkingNewsEntity(NewsScraperProviderResponse scrapedNews, IReadOnlyList<string> keywords, DebunkingNewsAddRequest customAttributes = null)
        {
            var allKeywords = customAttributes?.Keywords != null && customAttributes.Keywords.Any() ?
                keywords.Union(customAttributes.Keywords).ToList().AsReadOnly() :
                keywords;

            var entity = new DebunkingNews
            {
                Keywords = string.Join(',', allKeywords),
                Link = scrapedNews.RequestedUri.AbsoluteUri,
                NewsCaption = customAttributes?.Caption ?? scrapedNews.GetCaption(),
                PublisherName = scrapedNews.WebSiteName,
                Title = customAttributes?.Title ?? scrapedNews.Title
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

        /// <summary>
        /// Add the given debunking news to the repository
        /// </summary>
        /// <param name="request">Debunking news</param>
        /// <returns></returns>
        Task AddDebunkingNewsAsync(DebunkingNewsAddRequest request);
    }
}
