using System.Collections.Concurrent;
using System.Threading.Tasks;
using CriThink.Server.Core.Providers;

namespace CriThink.Server.Providers.NewsAnalyzer.Analyzers
{
    internal abstract class BaseNewsAnalyzer : IAnalyzer<NewsAnalysisProviderResult>
    {
        protected readonly ConcurrentQueue<Task<NewsAnalysisProviderResult>> Queue;

        private IAnalyzer<NewsAnalysisProviderResult> _nextAnalyzer;

        protected BaseNewsAnalyzer(NewsScraperProviderResponse scrapedNews, ConcurrentQueue<Task<NewsAnalysisProviderResult>> queue)
        {
            ScrapedNews = scrapedNews;
            Queue = queue;
        }

        protected NewsScraperProviderResponse ScrapedNews { get; }

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
