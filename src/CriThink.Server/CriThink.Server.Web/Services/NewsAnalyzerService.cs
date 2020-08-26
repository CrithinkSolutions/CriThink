using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CriThink.Common.Endpoints.DTOs.NewsAnalyzer;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Queries;
using CriThink.Server.Providers.DomainAnalyzer;
using CriThink.Server.Providers.NewsAnalyzer;
using CriThink.Server.Providers.NewsAnalyzer.Managers;
using CriThink.Server.Web.Facades;
using MediatR;

namespace CriThink.Server.Web.Services
{
    public class NewsAnalyzerService : INewsAnalyzerService
    {
        private readonly INewsScraperManager _newsScraperManager;
        private readonly INewsAnalyzerFacade _newsAnalyzerFacade;
        private readonly IDomainAnalyzerFacade _domainAnalyzerFacade;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public NewsAnalyzerService(INewsScraperManager newsScraperManager, INewsAnalyzerFacade newsAnalyzerFacade, IDomainAnalyzerFacade domainAnalyzerFacade, IMediator mediator, IMapper mapper)
        {
            _newsScraperManager = newsScraperManager ?? throw new ArgumentNullException(nameof(newsScraperManager));
            _newsAnalyzerFacade = newsAnalyzerFacade ?? throw new ArgumentNullException(nameof(newsAnalyzerFacade));
            _domainAnalyzerFacade = domainAnalyzerFacade ?? throw new ArgumentNullException(nameof(domainAnalyzerFacade));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<NewsAnalyzerResponse> HasUriHttpsSupportAsync(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            var analysisResponse = await _domainAnalyzerFacade.HasHttpsSupportAsync(uri).ConfigureAwait(false);
            if (analysisResponse.HasError)
                throw analysisResponse.Exception;

            var response = _mapper.Map<DomainAnalysisProviderResult, NewsAnalyzerResponse>(analysisResponse);
            return response;
        }

        public async Task<NewsAnalyzerResponse> AnalyzeDomainAsync(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            var analysisResponse = await _domainAnalyzerFacade.GetDomainInfoAsync(uri).ConfigureAwait(false);

            if (analysisResponse.HasError)
                throw analysisResponse.Exception;

            var response = _mapper.Map<DomainAnalysisProviderResult, NewsAnalyzerResponse>(analysisResponse);
            return response;
        }

        public async Task<NewsAnalyzerResponse[]> GetCompleteAnalysisAsync(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            var analysisResponses = await _domainAnalyzerFacade.GetCompleteAnalysisAsync(uri).ConfigureAwait(false);

            return analysisResponses
                .Select(r => _mapper.Map<DomainAnalysisProviderResult, NewsAnalyzerResponse>(r))
                .ToArray();
        }

        public async Task<ScrapeNewsResponse> NewsCheckSpellingAsync(Uri uri)
        {
            var scraperResponse = await _newsScraperManager.ScrapeNewsWebPage(uri).ConfigureAwait(false);

            var response = _mapper.Map<NewsScraperProviderResponse, ScrapeNewsResponse>(scraperResponse);
            return response;
        }

        public async Task<NewsAnalyzerResponse> AnalyzeNewsSentimentAsync(Uri uri)
        {
            var scraperResponse = await _newsScraperManager.ScrapeNewsWebPage(uri).ConfigureAwait(false);

            var analysisResponse = await _newsAnalyzerFacade.GetNewsSentimentAsync(scraperResponse).ConfigureAwait(false);
            if (analysisResponse.HasError)
                throw analysisResponse.Exception;

            var response = _mapper.Map<NewsAnalysisProviderResult, NewsAnalyzerResponse>(analysisResponse);
            return response;
        }

        public async Task<IList<DemoNewsResponse>> GetDemoNewsListAsync()
        {
            var query = new GetAllDemoNewsQuery();
            var response = await _mediator.Send(query).ConfigureAwait(false);

            if (response is IEnumerable<DemoNews> newsList)
            {
                return _mapper.Map<IEnumerable<DemoNews>, IList<DemoNewsResponse>>(newsList);
            }

            throw new InvalidOperationException($"Invalid result from '{nameof(GetAllDemoNewsQuery)}' query");
        }

        public async Task AddDemoNewsAsync(DemoNewsAddRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var entity = _mapper.Map<DemoNewsAddRequest, DemoNews>(request);

            var _ = await _mediator.Send(entity).ConfigureAwait(false);
        }
    }

    public interface INewsAnalyzerService
    {
        /// <summary>
        /// Returns a status to identify if the provided URI has HTTPS support or not
        /// </summary>
        /// <param name="uri"><see cref="Uri"/> to analyze</param>
        /// <returns>A response indicating the analysis result</returns>
        Task<NewsAnalyzerResponse> HasUriHttpsSupportAsync(Uri uri);

        /// <summary>
        /// Analyze the domain of the given <see cref="Uri"/>
        /// </summary>
        /// <param name="uri"><see cref="Uri"/> to analyze</param>
        /// <returns>A response indicating the analysis result</returns>
        Task<NewsAnalyzerResponse> AnalyzeDomainAsync(Uri uri);

        /// <summary>
        /// Perform an analysis of the given <see cref="Uri"/> using all the available analyzers
        /// </summary>
        /// <param name="uri"><see cref="Uri"/> to analyze</param>
        /// <returns>A response indicating the analysis result</returns>
        Task<NewsAnalyzerResponse[]> GetCompleteAnalysisAsync(Uri uri);

        /// <summary>
        /// Scrape the given news parsing useful information
        /// </summary>
        /// <param name="uri">News uri</param>
        /// <returns>News details such as author, title, body, etc.</returns>
        Task<ScrapeNewsResponse> NewsCheckSpellingAsync(Uri uri);

        /// <summary>
        /// Analyze the given news content and gives scores
        /// </summary>
        /// <param name="uri">News uri</param>
        /// <returns>News sentiment scores</returns>
        Task<NewsAnalyzerResponse> AnalyzeNewsSentimentAsync(Uri uri);

        /// <summary>
        /// Get the news list for the demo
        /// </summary>
        /// <returns>List of news</returns>
        Task<IList<DemoNewsResponse>> GetDemoNewsListAsync();

        /// <summary>
        /// Add the given news
        /// </summary>
        /// <param name="request">News to add</param>
        /// <returns>Awaitable task</returns>
        Task AddDemoNewsAsync(DemoNewsAddRequest request);
    }
}
