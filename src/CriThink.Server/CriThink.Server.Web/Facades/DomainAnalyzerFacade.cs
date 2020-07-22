using System;
using System.Linq;
using System.Threading.Tasks;
using CriThink.Server.Providers.DomainAnalyzer;
using CriThink.Server.Providers.DomainAnalyzer.Builders;
using CriThink.Server.Providers.DomainAnalyzer.Providers;

namespace CriThink.Server.Web.Facades
{
    public class DomainAnalyzerFacade : IDomainAnalyzerFacade
    {
        private readonly IDomainAnalyzerProvider _domainAnalyzerProvider;

        public DomainAnalyzerFacade(IDomainAnalyzerProvider domainAnalyzerProvider)
        {
            _domainAnalyzerProvider = domainAnalyzerProvider ?? throw new ArgumentNullException(nameof(domainAnalyzerProvider));
        }

        public async Task<AnalysisResponse[]> GetCompleteAnalysisAsync(Uri uri)
        {
            var builder = new DomainAnalyzerBuilder()
                .SetUri(uri)
                .EnableHttpsAnalysis()
                .EnableDomainAnalysis();

            var responses = await MakeRequestAsync(builder).ConfigureAwait(false);
            return responses;
        }

        public async Task<AnalysisResponse> HasHttpsSupportAsync(Uri uri)
        {
            var builder = new DomainAnalyzerBuilder()
                .SetUri(uri)
                .EnableHttpsAnalysis();

            var responses = await MakeRequestAsync(builder).ConfigureAwait(false);
            return responses.FirstOrDefault(r => r.Analysis == AnalysisType.HTTPS);
        }

        public async Task<AnalysisResponse> GetDomainInfoAsync(Uri uri)
        {
            var builder = new DomainAnalyzerBuilder()
                .SetUri(uri)
                .EnableDomainAnalysis();

            var responses = await MakeRequestAsync(builder).ConfigureAwait(false);
            return responses.FirstOrDefault(r => r.Analysis == AnalysisType.WhoIs);
        }

        private Task<AnalysisResponse[]> MakeRequestAsync(DomainAnalyzerBuilder builder)
        {
            var analyzerTasks = _domainAnalyzerProvider.StartAnalyzer(builder);
            return Task.WhenAll(analyzerTasks);
        }
    }

    public interface IDomainAnalyzerFacade
    {
        /// <summary>
        /// Ask if the given <see cref="Uri" /> has HTTPS support
        /// </summary>
        /// <param name="uri"><see cref="Uri"/> to analyze</param>
        /// <returns>Returns <see cref="AnalysisResponse"/> with analysis results</returns>
        Task<AnalysisResponse> HasHttpsSupportAsync(Uri uri);

        /// <summary>
        /// Get the domain info of the given <see cref="Uri"/>
        /// </summary>
        /// <param name="uri"><see cref="Uri"/> to analyze</param>
        /// <returns>Returns <see cref="AnalysisResponse"/> with analysis results</returns>
        Task<AnalysisResponse> GetDomainInfoAsync(Uri uri);

        /// <summary>
        /// Perform an analysis of the given <see cref="Uri"/> using all the NewsAnalyzer available
        /// </summary>
        /// <param name="uri"><see cref="Uri"/> to analyze</param>
        /// <returns>Returns <see cref="AnalysisResponse"/> with analysis results</returns>
        Task<AnalysisResponse[]> GetCompleteAnalysisAsync(Uri uri);
    }
}
