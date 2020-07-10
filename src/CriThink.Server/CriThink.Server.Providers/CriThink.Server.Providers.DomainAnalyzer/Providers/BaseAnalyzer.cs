using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace CriThink.Server.Providers.DomainAnalyzer.Providers
{
    internal abstract class BaseAnalyzer : IAnalyzer
    {
        protected readonly ConcurrentQueue<Task<AnalysisResponse>> Queue;

        private IAnalyzer _nextAnalyzer;

        protected BaseAnalyzer(Uri uri, ConcurrentQueue<Task<AnalysisResponse>> queue)
        {
            Uri = uri;
            Queue = queue;
        }

        protected Uri Uri { get; }

        public IAnalyzer SetNext(IAnalyzer analyzer)
        {
            _nextAnalyzer = analyzer;
            return _nextAnalyzer;
        }

        public virtual Task<AnalysisResponse>[] AnalyzeAsync()
        {
            var nextAnalyzer = _nextAnalyzer?.AnalyzeAsync();
            return nextAnalyzer ?? Queue.ToArray();
        }
    }

    internal interface IAnalyzer
    {
        /// <summary>
        /// Add the next <see cref="IAnalyzer"/> to the chain
        /// </summary>
        /// <param name="analyzer"><see cref="IAnalyzer"/> instance</param>
        /// <returns>Returns the given <see cref="IAnalyzer"/> instance</returns>
        IAnalyzer SetNext(IAnalyzer analyzer);

        /// <summary>
        /// Start the analyzer
        /// </summary>
        /// <returns>Returns the list of analysis responses</returns>
        Task<AnalysisResponse>[] AnalyzeAsync();
    }
}
