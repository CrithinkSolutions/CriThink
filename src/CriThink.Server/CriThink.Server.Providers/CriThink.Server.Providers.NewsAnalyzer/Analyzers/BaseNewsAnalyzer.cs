using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace CriThink.Server.Providers.NewsAnalyzer.Analyzers
{
    internal abstract class BaseNewsAnalyzer : INewsAnalyzer
    {
        protected readonly ConcurrentQueue<Task<NewsAnalysisProviderResponse>> Queue;

        private INewsAnalyzer _nextAnalyzer;

        protected BaseNewsAnalyzer(NewsScraperProviderResponse scrapedNews, ConcurrentQueue<Task<NewsAnalysisProviderResponse>> queue)
        {
            ScrapedNews = scrapedNews;
            Queue = queue;
        }

        protected NewsScraperProviderResponse ScrapedNews { get; }

        public INewsAnalyzer SetNext(INewsAnalyzer analyzer)
        {
            _nextAnalyzer = analyzer;
            return _nextAnalyzer;
        }

        public virtual Task<NewsAnalysisProviderResponse>[] AnalyzeAsync()
        {
            var nextAnalyzer = _nextAnalyzer?.AnalyzeAsync();
            return nextAnalyzer ?? Queue.ToArray();
        }
    }

    internal interface INewsAnalyzer
    {
        /// <summary>
        /// Add the next <see cref="INewsAnalyzer"/> to the chain
        /// </summary>
        /// <param name="analyzer"><see cref="INewsAnalyzer"/> instance</param>
        /// <returns>Returns the given <see cref="INewsAnalyzer"/> instance</returns>
        INewsAnalyzer SetNext(INewsAnalyzer analyzer);

        /// <summary>
        /// Start the analyzer
        /// </summary>
        /// <returns>Returns the list of analysis responses</returns>
        Task<NewsAnalysisProviderResponse>[] AnalyzeAsync();
    }
}
