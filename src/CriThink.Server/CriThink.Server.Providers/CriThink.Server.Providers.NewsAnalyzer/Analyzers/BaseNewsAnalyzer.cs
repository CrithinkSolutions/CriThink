using System.Collections.Concurrent;
using System.Threading.Tasks;
using CriThink.Server.Providers.Common;

namespace CriThink.Server.Providers.NewsAnalyzer.Analyzers
{
    internal abstract class BaseNewsAnalyzer : IAnalyzer<NewsAnalysisProviderResult>
    {
        private IAnalyzer<NewsAnalysisProviderResult> _nextAnalyzer;

        internal NewsScraperProviderResponse ScrapedNews { get; set; }

        internal ConcurrentQueue<Task<NewsAnalysisProviderResult>> Queue { get; set; }

        public IAnalyzer<NewsAnalysisProviderResult> SetNext(IAnalyzer<NewsAnalysisProviderResult> analyzer)
        {
            _nextAnalyzer = analyzer;
            return _nextAnalyzer;
        }

        public virtual Task<NewsAnalysisProviderResult>[] AnalyzeAsync()
        {
            var nextAnalyzer = _nextAnalyzer?.AnalyzeAsync();
            return nextAnalyzer ?? Queue.ToArray();
        }
    }
}
