using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using CriThink.Server.Core.Commands;
using CriThink.Server.Core.Interfaces;
using CriThink.Server.Core.Queries;
using CriThink.Server.Core.Responses;
using CriThink.Server.Providers.EmailSender.Services;
using CriThink.Server.Providers.NewsAnalyzer.Managers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Core.Services
{
    public class NewsSourceService : INewsSourceService
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IEmailSenderService _emailSenderService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly INewsScraperManager _newsScraperManager;
        private readonly ILogger<NewsSourceService> _logger;

        public NewsSourceService(IMediator mediator, IMapper mapper, IEmailSenderService emailSenderService, IHttpContextAccessor httpContext, INewsScraperManager newsScraperManager, ILogger<NewsSourceService> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _emailSenderService = emailSenderService ?? throw new ArgumentNullException(nameof(emailSenderService));
            _httpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            _newsScraperManager = newsScraperManager ?? throw new ArgumentNullException(nameof(newsScraperManager));
            _logger = logger;
        }

        public async Task AddSourceAsync(NewsSourceAddRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var uri = new Uri(request.Uri, UriKind.Absolute);
            var authenticity = _mapper.Map<NewsSourceClassification, NewsSourceAuthenticity>(request.Classification);

            var command = new CreateNewsSourceCommand(uri, authenticity);
            _ = await _mediator.Send(command).ConfigureAwait(false);
        }

        public async Task RemoveNewsSourceAsync(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            var command = new RemoveNewsSourceCommand(uri);
            _ = await _mediator.Send(command).ConfigureAwait(false);
        }

        public async Task<NewsSourceSearchResponse> SearchNewsSourceAsync(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            var query = new SearchNewsSourceQuery(uri);
            var queryResponse = await _mediator.Send(query).ConfigureAwait(false);

            if (queryResponse is null)
                return null;

            var response = _mapper.Map<SearchNewsSourceQueryResponse, NewsSourceSearchResponse>(queryResponse);
            return response;
        }

        public async Task<NewsSourceSearchWithDebunkingNewsResponse> SearchNewsSourceWithAlertAsync(Uri uri)
        {
            var relatedDebunkingNewsResponse = new List<NewsSourceRelatedDebunkingNewsResponse>();

            var searchResponse = await SearchNewsSourceAsync(uri).ConfigureAwait(false);

            if (searchResponse is null)
            {
                await SendUnknownDomainAlertEmailAsync(uri).ConfigureAwait(false);
                return null;
            }

            if (searchResponse.Classification == NewsSourceClassification.Conspiracist ||
                searchResponse.Classification == NewsSourceClassification.FakeNews ||
                searchResponse.Classification == NewsSourceClassification.Suspicious)
            {
                try
                {
                    var relatedDebunkingNewsCollection = await GetRelatedDebunkingNewsAsync(uri)
                        .ConfigureAwait(false);

                    foreach (var relatedDNews in relatedDebunkingNewsCollection)
                    {
                        var response = _mapper.Map<GetAllDebunkingNewsByKeywordsQueryResponse, NewsSourceRelatedDebunkingNewsResponse>(relatedDNews);
                        relatedDebunkingNewsResponse.Add(response);
                    }
                }
                catch (InvalidOperationException) { }
            }

            return new NewsSourceSearchWithDebunkingNewsResponse
            {
                Classification = searchResponse.Classification,
                Description = searchResponse.Description,
                RelatedDebunkingNews = relatedDebunkingNewsResponse,
            };
        }

        public async Task<NewsSourceGetAllResponse> GetAllNewsSourcesAsync(NewsSourceGetAllRequest request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            var sourceFilter = _mapper.Map<NewsSourceGetAllFilterRequest, GetAllNewsSourceFilter>(request.Filter);

            var query = new GetAllNewsSourceQuery(request.PageSize, request.PageIndex, sourceFilter);
            var queryResponse = await _mediator.Send(query).ConfigureAwait(false);

            var dtos = new List<NewsSourceGetResponse>();
            foreach (var newsSource in queryResponse.Take(request.PageSize))
            {
                var dto = _mapper.Map<GetAllNewsSourceQueryResponse, NewsSourceGetResponse>(newsSource);
                dtos.Add(dto);
            }

            var response = new NewsSourceGetAllResponse(dtos, queryResponse.Count > request.PageSize);
            return response;
        }

        private async Task SendUnknownDomainAlertEmailAsync(Uri uri)
        {
            var email = _httpContext.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;

            await _emailSenderService.SendUnknownDomainAlertEmailAsync(uri.ToString(), email).ConfigureAwait(false);
            var command = new CreateUnknownNewsSourceCommand(uri);
            await _mediator.Send(command).ConfigureAwait(false);
        }

        private async Task<IList<GetAllDebunkingNewsByKeywordsQueryResponse>> GetRelatedDebunkingNewsAsync(Uri uri)
        {
            try
            {
                var scrapeAnalysis = await _newsScraperManager.ScrapeNewsWebPage(uri)
                    .ConfigureAwait(false);

                var keywords = await _newsScraperManager.GetKeywordsFromNewsAsync(scrapeAnalysis)
                    .ConfigureAwait(false);

                var dNewsByKeywordsQuery = new GetAllDebunkingNewsByKeywordsQuery(keywords);
                return await _mediator.Send(dNewsByKeywordsQuery);
            }
            catch (InvalidOperationException ex)
            {
                _logger?.LogWarning(ex, "The given URL is not readable");
                throw;
            }
        }
    }
}
