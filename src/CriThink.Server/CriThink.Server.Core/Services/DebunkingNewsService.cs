using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CriThink.Common.Endpoints.DTOs.Admin;
using CriThink.Server.Core.Commands;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Facades;
using CriThink.Server.Core.Interfaces;
using CriThink.Server.Core.Queries;
using CriThink.Server.Core.Responses;
using CriThink.Server.Providers.DebunkNewsFetcher;
using CriThink.Server.Providers.NewsAnalyzer;
using CriThink.Server.Providers.NewsAnalyzer.Managers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Core.Services
{
    public class DebunkingNewsService : IDebunkingNewsService
    {
        private readonly IDebunkNewsFetcherFacade _debunkNewsFetcherFacade;
        private readonly INewsScraperManager _newsScraperManager;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<DebunkingNewsService> _logger;

        public DebunkingNewsService(IDebunkNewsFetcherFacade debunkNewsFetcherFacade, INewsScraperManager newsScraperManager, IMediator mediator, IMapper mapper, ILogger<DebunkingNewsService> logger)
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

            var entity = BuildDebunkingNewsEntity(scrapedNews, keywords, request.Title, request.Caption, request.Keywords);

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

        public async Task<DebunkingNewsGetAllResponse> GetAllDebunkingNewsAsync(DebunkingNewsGetAllRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var query = new GetAllDebunkingNewsQuery(request.PageSize + 1, request.PageIndex);
            var debunkingNewsCollection = await _mediator.Send(query).ConfigureAwait(false);

            var dtos = new List<DebunkingNewsGetResponse>();
            foreach (var debunkingNews in debunkingNewsCollection.Take(request.PageSize))
            {
                var dto = _mapper.Map<GetAllDebunkingNewsQueryResponse, DebunkingNewsGetResponse>(debunkingNews);
                dtos.Add(dto);
            }

            var response = new DebunkingNewsGetAllResponse(dtos, debunkingNewsCollection.Count > request.PageSize);
            return response;
        }

        public async Task<DebunkingNewsGetDetailsResponse> GetDebunkingNewsAsync(DebunkingNewsGetRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var query = new GetDebunkingNewsQuery(request.Id);
            var debunkingNews = await _mediator.Send(query).ConfigureAwait(false);

            var dto = _mapper.Map<DebunkingNews, DebunkingNewsGetDetailsResponse>(debunkingNews);
            return dto;
        }

        public async Task<TriggerLogsGetAllResponse> GetAllTriggerLogsAsync(TriggerLogsGetAllRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var query = new GetAllTriggerLogsQuery(request.PageSize + 1, request.PageIndex);
            var triggerLogs = await _mediator.Send(query).ConfigureAwait(false);
            var dtos = new List<TriggerLogGetResponse>();
            foreach (var log in triggerLogs.Take(request.PageSize))
            {
                var dto = _mapper.Map<GetAllTriggerLogQueryResponse, TriggerLogGetResponse>(log);
                dtos.Add(dto);
            }

            var response = new TriggerLogsGetAllResponse(dtos, triggerLogs.Count > request.PageSize);
            return response;
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

        private static DebunkingNews BuildDebunkingNewsEntity(NewsScraperProviderResponse scrapedNews, IReadOnlyList<string> keywords,
            string customTitle = null, string customCaption = null, IReadOnlyCollection<string> customKeywords = null)
        {
            var allKeywords = customKeywords != null && customKeywords.Any() ?
                keywords.Union(customKeywords).ToList().AsReadOnly() :
                keywords;

            var entity = new DebunkingNews
            {
                Keywords = string.Join(',', allKeywords),
                Link = scrapedNews.RequestedUri.AbsoluteUri,
                NewsCaption = customCaption ?? scrapedNews.GetCaption(),
                PublisherName = scrapedNews.WebSiteName,
                Title = customTitle ?? scrapedNews.Title
            };

            if (scrapedNews.Date.HasValue)
                entity.PublishingDate = scrapedNews.Date.Value;

            return entity;
        }
    }
}
