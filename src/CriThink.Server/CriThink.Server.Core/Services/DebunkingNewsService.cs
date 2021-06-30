using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CriThink.Common.Endpoints.DTOs.Admin;
using CriThink.Server.Core.Commands;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Exceptions;
using CriThink.Server.Core.Facades;
using CriThink.Server.Core.Interfaces;
using CriThink.Server.Core.Queries;
using CriThink.Server.Core.Responses;
using CriThink.Server.Providers.DebunkingNewsFetcher;
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

            var publisherQuery = new GetDebunkingNewsPublisherByIdQuery(request.PublisherId);
            var publisher = await _mediator.Send(publisherQuery).ConfigureAwait(false);

            var entity = BuildDebunkingNewsEntity(scrapedNews, keywords, publisher, request.Title, request.Caption, request.ImageLink, request.Keywords);

            var command = new CreateDebunkingNewsCommand(new[] { entity });
            _ = await _mediator.Send(command).ConfigureAwait(false);
        }

        public async Task DeleteDebunkingNewsAsync(SimpleDebunkingNewsRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var command = new DeleteDebunkingNewsCommand(request.Id);
            _ = await _mediator.Send(command).ConfigureAwait(false);
        }

        public async Task UpdateDebunkingNewsAsync(DebunkingNewsUpdateRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var command = new UpdateDebunkingNewsCommand(request.Id, request.Title, request.Caption, request.Link, request.ImageLink, request.Keywords);
            _ = await _mediator.Send(command).ConfigureAwait(false);
        }

        public async Task UpdateRepositoryAsync()
        {
            var logOperationCommand = new CreateTriggerLogCommand();

            try
            {
                var lastFetchTimeQuery = new GetLastDebunkinNewsFetchTimeQuery();
                var lastDateTask = _mediator.Send(lastFetchTimeQuery);
                var debunkingNewsTask = _debunkNewsFetcherFacade.FetchDebunkingNewsAsync();

                await Task.WhenAll(lastDateTask, debunkingNewsTask).ConfigureAwait(false);

                var lastSuccessfullFetchDate = lastDateTask.Result;
                var debunkingNewsCollection = debunkingNewsTask.Result;

                if (!debunkingNewsCollection.Any())
                    return;

                DebunkingNewsPublisher publisherOpen = null;
                DebunkingNewsPublisher publisherChannel4 = null;
                DebunkingNewsPublisher publisherFullFact = null;
                DebunkingNewsPublisher publisherFactaNews = null;


                var publishersTask = Task.Run(async () =>
                {
                    var publisherOpenQuery = new GetDebunkingNewsPublisherByNameQuery(EntityConstants.OpenOnline);
                    var publisherChannel4Query = new GetDebunkingNewsPublisherByNameQuery(EntityConstants.Channel4);
                    var publisherFullFactQuery = new GetDebunkingNewsPublisherByNameQuery(EntityConstants.FullFact);
                    var publisherFactaNewsQuery = new GetDebunkingNewsPublisherByNameQuery(EntityConstants.FactaNews);

                    publisherOpen = await _mediator.Send(publisherOpenQuery).ConfigureAwait(false);
                    publisherChannel4 = await _mediator.Send(publisherChannel4Query).ConfigureAwait(false);
                    publisherFullFact = await _mediator.Send(publisherFullFactQuery).ConfigureAwait(false);
                    publisherFactaNews = await _mediator.Send(publisherFactaNewsQuery).ConfigureAwait(false);
                });

                var scrapeTask = ScrapeDebunkingNewsCollectionAsync(debunkingNewsCollection, lastSuccessfullFetchDate);

                await Task.WhenAll(publishersTask, scrapeTask).ConfigureAwait(false);

                var debunkedNewsCollection = scrapeTask.Result;

                if (!debunkedNewsCollection.Any())
                    return;

                foreach (var dNews in debunkedNewsCollection)
                {
                    if (dNews.Link.Contains(EntityConstants.OpenOnlineLink, StringComparison.InvariantCultureIgnoreCase))
                        dNews.Publisher = publisherOpen;
                    else if (dNews.Link.Contains(EntityConstants.Channel4Link, StringComparison.InvariantCultureIgnoreCase))
                        dNews.Publisher = publisherChannel4;
                    else if (dNews.Link.Contains(EntityConstants.FullFactLink, StringComparison.InvariantCultureIgnoreCase))
                        dNews.Publisher = publisherFullFact;
                    else if (dNews.Link.Contains(EntityConstants.FactaNewsLink, StringComparison.InvariantCultureIgnoreCase))
                        dNews.Publisher = publisherFactaNews;
                }

                var addNewsCommand = new CreateDebunkingNewsCommand(debunkedNewsCollection);
                _ = await _mediator.Send(addNewsCommand).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error to fetch debunking news");
                logOperationCommand = new CreateTriggerLogCommand(ex.Message);
                throw;
            }
            finally
            {
                _ = await _mediator.Send(logOperationCommand).ConfigureAwait(false);
            }
        }

        public async Task<DebunkingNewsGetAllResponse> GetAllDebunkingNewsAsync(DebunkingNewsGetAllRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var filter = _mapper.Map<DebunkingNewsGetAllLanguageFilterRequests, GetAllDebunkingNewsLanguageFilters>(request.LanguageFilters);

            var query = new GetAllDebunkingNewsQuery(request.PageSize, request.PageIndex, filter);
            var debunkingNewsCollection = await _mediator.Send(query).ConfigureAwait(false);

            var dtos = debunkingNewsCollection
                .Take(request.PageSize)
                .Select(debunkingNews => _mapper.Map<GetAllDebunkingNewsQueryResponse, DebunkingNewsGetResponse>(debunkingNews))
                .ToList();

            var response = new DebunkingNewsGetAllResponse(dtos, debunkingNewsCollection.Count > request.PageSize);
            return response;
        }

        public async Task<DebunkingNewsGetDetailsResponse> GetDebunkingNewsAsync(DebunkingNewsGetRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var query = new GetDebunkingNewsQuery(request.Id);
            var debunkingNews = await _mediator.Send(query).ConfigureAwait(false);
            if (debunkingNews is null)
                throw new ResourceNotFoundException($"Can't find a debunking news with id '{request.Id}'");

            var dto = _mapper.Map<DebunkingNews, DebunkingNewsGetDetailsResponse>(debunkingNews);
            return dto;
        }

        public async Task<TriggerLogsGetAllResponse> GetAllTriggerLogsAsync(TriggerLogsGetAllRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var query = new GetAllTriggerLogsQuery(request.PageSize, request.PageIndex);
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

                        if (scrapedNews.Date is null)
                            scrapedNews.SetPublishingDate(response.PublishingDate);

                        var keywords = await GetNewsKeywordsAsync(scrapedNews)
                            .ConfigureAwait(false);

                        var entity = BuildDebunkingNewsEntity(scrapedNews: scrapedNews, keywords: keywords, imageLink: response.ImageLink);

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

        private Task<NewsScraperProviderResponse> ScrapeNewsAsync(Uri link) =>
            _newsScraperManager.ScrapeNewsWebPage(link);

        private Task<IReadOnlyList<string>> GetNewsKeywordsAsync(NewsScraperProviderResponse scrapedNews) =>
            _newsScraperManager.GetKeywordsFromNewsAsync(scrapedNews);

        private static DebunkingNews BuildDebunkingNewsEntity(NewsScraperProviderResponse scrapedNews, IReadOnlyList<string> keywords,
            DebunkingNewsPublisher publisher = null, string customTitle = null, string customCaption = null, string imageLink = null,
            IReadOnlyCollection<string> customKeywords = null)
        {
            var allKeywords = customKeywords != null && customKeywords.Any() ?
                keywords.Union(customKeywords).ToList().AsReadOnly() :
                keywords;

            var entity = new DebunkingNews
            {
                Keywords = string.Join(',', allKeywords),
                Link = scrapedNews.RequestedUri.AbsoluteUri,
                NewsCaption = customCaption ?? scrapedNews.GetCaption(),
                Title = customTitle ?? scrapedNews.Title,
                ImageLink = imageLink
            };

            if (scrapedNews.Date.HasValue)
                entity.PublishingDate = scrapedNews.Date.Value;

            if (publisher is not null)
                entity.Publisher = publisher;

            return entity;
        }
    }
}
