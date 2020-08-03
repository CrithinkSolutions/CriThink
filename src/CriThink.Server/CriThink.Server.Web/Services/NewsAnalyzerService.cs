using System;
using System.Threading.Tasks;
using AutoMapper;
using CriThink.Common.Endpoints.DTOs.NewsAnalyzer;
using CriThink.Server.Providers.NewsAnalyzer;
using CriThink.Server.Providers.NewsAnalyzer.Managers;
using CriThink.Server.Web.Facades;

namespace CriThink.Server.Web.Services
{
    public class NewsAnalyzerService : INewsAnalyzerService
    {
        private readonly INewsScraperManager _newsScraperManager;
        private readonly INewsAnalyzerFacade _newsAnalyzerFacade;
        private readonly IMapper _mapper;

        public NewsAnalyzerService(INewsScraperManager newsScraperManager, INewsAnalyzerFacade newsAnalyzerFacade, IMapper mapper)
        {
            _newsScraperManager = newsScraperManager ?? throw new ArgumentNullException(nameof(newsScraperManager));
            _newsAnalyzerFacade = newsAnalyzerFacade ?? throw new ArgumentNullException(nameof(newsAnalyzerFacade));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
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

            var response = _mapper.Map<NewsAnalysisProviderResponse, NewsAnalyzerResponse>(analysisResponse);
            return response;
        }
    }

    public interface INewsAnalyzerService
    {
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
    }
}
