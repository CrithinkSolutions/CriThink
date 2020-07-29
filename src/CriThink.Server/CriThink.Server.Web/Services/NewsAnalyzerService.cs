using System;
using System.Threading.Tasks;
using AutoMapper;
using CriThink.Common.Endpoints.DTOs.NewsAnalyzer;
using CriThink.Server.Providers.NewsAnalyzer.Managers;
using CriThink.Server.Providers.NewsAnalyzer.Responses;

namespace CriThink.Server.Web.Services
{
    public class NewsAnalyzerService : INewsAnalyzerService
    {
        private readonly INewsScraperManager _newsScraperManager;
        private readonly IMapper _mapper;

        public NewsAnalyzerService(INewsScraperManager newsScraperManager, IMapper mapper)
        {
            _newsScraperManager = newsScraperManager ?? throw new ArgumentNullException(nameof(newsScraperManager));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ScrapeNewsResponse> NewsCheckSpellingAsync(Uri uri)
        {
            var scraperResponse = await _newsScraperManager.ScrapeNewsWebPage(uri).ConfigureAwait(false);

            var response = _mapper.Map<NewsScraperResponse, ScrapeNewsResponse>(scraperResponse);
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
    }
}
