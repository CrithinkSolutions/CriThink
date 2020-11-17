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

            await AddDebunkingNewsAsync(request.Link, request.Title, request.Caption, request.Keywords).ConfigureAwait(false);
        }

        public async Task AddDebunkingNewsAsync(AddNewsViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            await AddDebunkingNewsAsync(viewModel.Link, viewModel.Title, viewModel.Caption, viewModel.Keywords).ConfigureAwait(false);
        }

        public async Task DeleteDebunkingNewsAsync(SimpleDebunkingNewsRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            await DeleteDebunkingNewsAsync(request.Id).ConfigureAwait(false);
        }

        public async Task DeleteDebunkingNewsAsync(SimpleDebunkingNewsViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            await DeleteDebunkingNewsAsync(viewModel.Id).ConfigureAwait(false);
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

        public async Task<IList<DebunkingNewsGetAllResponse>> GetAllDebunkingNewsAsync(DebunkingNewsGetAllRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return await GetDebunkingNewsAsync(request.PageSize, request.PageIndex).ConfigureAwait(false);
        }

        public async Task<IList<DebunkingNewsGetAllResponse>> GetAllDebunkingNewsAsync(SimplePaginationViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            return await GetDebunkingNewsAsync(viewModel.PageSize, viewModel.PageIndex).ConfigureAwait(false);
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

        private async Task AddDebunkingNewsAsync(string link, string customTitle = null, string customCaption = null, IReadOnlyCollection<string> customKeywords = null)
        {
            var scrapedNews = await _newsScraperManager.ScrapeNewsWebPage(new Uri(link))
                .ConfigureAwait(false);

            var keywords = await GetNewsKeywordsAsync(scrapedNews).ConfigureAwait(false);

            var entity = BuildDebunkingNewsEntity(scrapedNews, keywords, customTitle, customCaption, customKeywords);

            var command = new CreateDebunkingNewsCommand(new[] { entity });
            var _ = await _mediator.Send(command).ConfigureAwait(false);
        }

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

        private async Task DeleteDebunkingNewsAsync(Guid id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            var command = new DeleteDebunkingNewsCommand(id);
            var _ = await _mediator.Send(command).ConfigureAwait(false);
        }

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
