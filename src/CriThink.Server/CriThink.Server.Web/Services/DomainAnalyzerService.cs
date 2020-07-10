using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CriThink.Common.Endpoints.DTOs.NewsAnalyzer;
using CriThink.Server.Providers.DomainAnalyzer;
using CriThink.Server.Web.Facades;

namespace CriThink.Server.Web.Services
{
    public class DomainAnalyzerService : IDomainAnalyzerService
    {
        private readonly IDomainAnalyzerFacade _domainAnalyzerFacade;
        private readonly IMapper _mapper;

        public DomainAnalyzerService(IDomainAnalyzerFacade domainAnalyzerProvider, IMapper mapper)
        {
            _domainAnalyzerFacade = domainAnalyzerProvider ?? throw new ArgumentNullException(nameof(domainAnalyzerProvider));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<NewsAnalyzerResponse> HasUriHttpsSupportAsync(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            var analysisResponse = await _domainAnalyzerFacade.HasHttpsSupportAsync(uri).ConfigureAwait(false);
            if (analysisResponse.HasError)
                throw analysisResponse.Exception;

            var response = _mapper.Map<AnalysisResponse, NewsAnalyzerResponse>(analysisResponse);
            return response;
        }

        public async Task<NewsAnalyzerResponse> AnalyzeDomainAsync(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            var analysisResponse = await _domainAnalyzerFacade.GetDomainInfoAsync(uri).ConfigureAwait(false);

            if (analysisResponse.HasError)
                throw analysisResponse.Exception;

            var response = _mapper.Map<AnalysisResponse, NewsAnalyzerResponse>(analysisResponse);
            return response;
        }

        public async Task<NewsAnalyzerResponse[]> GetCompleteAnalysisAsync(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            var analysisResponses = await _domainAnalyzerFacade.GetCompleteAnalysisAsync(uri).ConfigureAwait(false);

            return analysisResponses
                .Select(r => _mapper.Map<AnalysisResponse, NewsAnalyzerResponse>(r))
                .ToArray();
        }
    }

    /// <summary>
    /// Offers API to analyze URL's domains
    /// </summary>
    public interface IDomainAnalyzerService
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
        /// Perform an analysis of the given <see cref="Uri"/> using all the NewsAnalyzer available
        /// </summary>
        /// <param name="uri"><see cref="Uri"/> to analyze</param>
        /// <returns>A response indicating the analysis result</returns>
        Task<NewsAnalyzerResponse[]> GetCompleteAnalysisAsync(Uri uri);
    }
}