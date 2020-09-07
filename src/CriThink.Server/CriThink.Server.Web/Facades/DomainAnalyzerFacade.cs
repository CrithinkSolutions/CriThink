using System;
using System.Linq;
using System.Threading.Tasks;
using CriThink.Server.Core.Providers;
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

        public async Task<DomainAnalysisProviderResult[]> GetCompleteAnalysisAsync(Uri uri)
        {
            var builder = new DomainAnalyzerBuilder()
                .SetUri(uri)
                .EnableHttpsAnalysis()
                .EnableDomainAnalysis();

            var responses = await MakeRequestAsync(builder).ConfigureAwait(false);
            return responses;
        }

        public async Task<DomainAnalysisProviderResult> HasHttpsSupportAsync(Uri uri)
        {
            var builder = new DomainAnalyzerBuilder()
                .SetUri(uri)
                .EnableHttpsAnalysis();

            var responses = await MakeRequestAsync(builder).ConfigureAwait(false);
            return responses.FirstOrDefault(r => r.NewsAnalysisType == NewsAnalysisType.HTTPS);
        }

        public async Task<DomainAnalysisProviderResult> GetDomainInfoAsync(Uri uri)
        {
            var builder = new DomainAnalyzerBuilder()
                .SetUri(uri)
                .EnableDomainAnalysis();

            var responses = await MakeRequestAsync(builder).ConfigureAwait(false);
            return responses.FirstOrDefault(r => r.NewsAnalysisType == NewsAnalysisType.WhoIs);
        }

        private Task<DomainAnalysisProviderResult[]> MakeRequestAsync(DomainAnalyzerBuilder builder)
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
        /// <returns>Returns <see cref="DomainAnalysisProviderResult"/> with analysis results</returns>
        Task<DomainAnalysisProviderResult> HasHttpsSupportAsync(Uri uri);

        /// <summary>
        /// Get the domain info of the given <see cref="Uri"/>
        /// </summary>
        /// <param name="uri"><see cref="Uri"/> to analyze</param>
        /// <returns>Returns <see cref="DomainAnalysisProviderResult"/> with analysis results</returns>
        Task<DomainAnalysisProviderResult> GetDomainInfoAsync(Uri uri);

        /// <summary>
        /// Perform an analysis of the given <see cref="Uri"/> using all the NewsAnalyzer available
        /// </summary>
        /// <param name="uri"><see cref="Uri"/> to analyze</param>
        /// <returns>Returns <see cref="DomainAnalysisProviderResult"/> with analysis results</returns>
        Task<DomainAnalysisProviderResult[]> GetCompleteAnalysisAsync(Uri uri);
    }
}
