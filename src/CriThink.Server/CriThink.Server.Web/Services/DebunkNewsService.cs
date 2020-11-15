using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CriThink.Common.Endpoints.DTOs.Admin;
using CriThink.Server.Core.Commands;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Queries;
using CriThink.Server.Core.Responses;
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
        private readonly IMapper _mapper;
        private readonly ILogger<DebunkNewsService> _logger;

        public DebunkNewsService(IDebunkNewsFetcherFacade debunkNewsFetcherFacade, INewsScraperManager newsScraperManager, IMediator mediator, IMapper mapper, ILogger<DebunkNewsService> logger)
        {
            _debunkNewsFetcherFacade = debunkNewsFetcherFacade ?? throw new ArgumentNullException(nameof(debunkNewsFetcherFacade));
            _newsScraperManager = newsScraperManager ?? throw new ArgumentNullException(nameof(newsScraperManager));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
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

        public async Task DeleteDebunkingNewsAsync(SimpleDebunkingNewsRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var command = new DeleteDebunkingNewsCommand(request.Id);
            var _ = await _mediator.Send(command).ConfigureAwait(false);
        }

        public async Task UpdateDebunkingNewsAsync(DebunkingNewsUpdateRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var command = new UpdateDebunkingNewsCommand(request.Id, request.Title, request.Caption, request.Link, request.Keywords);
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

        public async Task<IList<DebunkingNewsGetAllResponse>> GetAllDebunkingNewsAsync(int? pageSize, int? pageIndex)
        {
            if (pageSize == null || pageSize.HasValue)
                pageSize = 20;
            if (pageIndex == null || pageIndex.HasValue)
                pageIndex = 1;

            return await GetDebunkingNewsAsync(pageSize.Value, pageIndex.Value).ConfigureAwait(false);
        }

        public async Task<DebunkingNewsGetResponse> GetDebunkingNewsAsync(DebunkingNewsGetRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var query = new GetDebunkingNewsQuery(request.Id);
            var debunkingNews = await _mediator.Send(query).ConfigureAwait(false);

            var dto = _mapper.Map<DebunkingNews, DebunkingNewsGetResponse>(debunkingNews);
            return dto;
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

        private async Task<IList<DebunkingNewsGetAllResponse>> GetDebunkingNewsAsync(int pageSize, int pageIndex)
        {
            var query = new GetAllDebunkingNewsQuery(pageSize, pageIndex);
            var debunkingNewsCollection = await _mediator.Send(query).ConfigureAwait(false);

            var dtos = new List<DebunkingNewsGetAllResponse>();
            foreach (var debunkingNews in debunkingNewsCollection)
            {
                var dto = _mapper.Map<GetAllDebunkingNewsQueryResponse, DebunkingNewsGetAllResponse>(debunkingNews);
                dtos.Add(dto);
            }

            return dtos;
        }

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

        /// <summary>
        /// Delete the debunking news with the associated id
        /// </summary>
        /// <param name="request">Debunking news id</param>
        /// <returns></returns>
        Task DeleteDebunkingNewsAsync(SimpleDebunkingNewsRequest request);

        /// <summary>
        /// Update the debunking news
        /// </summary>
        /// <param name="request">The new list of keyworks</param>
        /// <returns></returns>
        Task UpdateDebunkingNewsAsync(DebunkingNewsUpdateRequest request);

        /// <summary>
        /// Get all the debunking news
        /// </summary>
        /// <param name="pageSize">Debunking news per page</param>
        /// <param name="pageIndex">Page index debunking news</param>
        /// <returns></returns>
        Task<IList<DebunkingNewsGetAllResponse>> GetAllDebunkingNewsAsync(int? pageSize, int? pageIndex);

        /// <summary>
        /// Get the specified debunking news
        /// </summary>
        /// <param name="request">Debunking news id</param>
        /// <returns></returns>
        Task<DebunkingNewsGetResponse> GetDebunkingNewsAsync(DebunkingNewsGetRequest request);
    }
}
