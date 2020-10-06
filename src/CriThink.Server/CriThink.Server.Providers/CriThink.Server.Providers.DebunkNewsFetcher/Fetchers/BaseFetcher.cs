using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using CriThink.Server.Core.Providers;

namespace CriThink.Server.Providers.DebunkNewsFetcher.Fetchers
{
    internal abstract class BaseFetcher : IAnalyzer<DebunkingNewsProviderResult>
    {
        protected readonly ConcurrentQueue<Task<DebunkingNewsProviderResult>> Queue;

        private IAnalyzer<DebunkingNewsProviderResult> _nextAnalyzer;

        protected BaseFetcher(ConcurrentQueue<Task<DebunkingNewsProviderResult>> queue)
        {
            Queue = queue ?? throw new ArgumentNullException(nameof(queue));
        }

        public IAnalyzer<DebunkingNewsProviderResult> SetNext(IAnalyzer<DebunkingNewsProviderResult> analyzer)
        {
            _nextAnalyzer = analyzer;
            return _nextAnalyzer;
        }

        public virtual Task<DebunkingNewsProviderResult>[] AnalyzeAsync()
        {
            var nextAnalyzer = _nextAnalyzer?.AnalyzeAsync();
            return nextAnalyzer ?? Queue.ToArray();
        }
    }
}
