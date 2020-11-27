using System;
using System.Linq;
using System.Threading.Tasks;
using CriThink.Server.Providers.Common;
using CriThink.Server.Providers.DomainAnalyzer;
using CriThink.Server.Providers.DomainAnalyzer.Builders;
using CriThink.Server.Providers.DomainAnalyzer.Providers;

namespace CriThink.Server.Core.Facades
{
    public class DomainAnalyzerFacade : IDomainAnalyzerFacade
    {
        private readonly DomainAnalyzerBuilder _builder;
        private readonly IDomainAnalyzerProvider _domainAnalyzerProvider;

        public DomainAnalyzerFacade(IDomainAnalyzerProvider domainAnalyzerProvider, DomainAnalyzerBuilder builder)
        {
            _domainAnalyzerProvider = domainAnalyzerProvider ?? throw new ArgumentNullException(nameof(domainAnalyzerProvider));
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        public async Task<DomainAnalysisProviderResult[]> GetCompleteAnalysisAsync(Uri uri)
        {
            var builder = _builder
                .SetUri(uri)
                .EnableHttpsAnalysis()
                .EnableDomainAnalysis();

            var responses = await MakeRequestAsync(builder).ConfigureAwait(false);
            return responses;
        }

        public async Task<DomainAnalysisProviderResult> HasHttpsSupportAsync(Uri uri)
        {
            var builder = _builder
                .SetUri(uri)
                .EnableHttpsAnalysis();

            var responses = await MakeRequestAsync(builder).ConfigureAwait(false);
            return responses.FirstOrDefault(r => r.NewsAnalysisType == NewsAnalysisType.HTTPS);
        }

        public async Task<DomainAnalysisProviderResult> GetDomainInfoAsync(Uri uri)
        {
            var builder = _builder
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
}
