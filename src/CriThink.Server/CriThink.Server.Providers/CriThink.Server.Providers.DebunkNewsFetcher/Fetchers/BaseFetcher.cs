using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using CriThink.Server.Core.Providers;

namespace CriThink.Server.Providers.DebunkNewsFetcher.Fetchers
{
    internal abstract class BaseFetcher : IAnalyzer<DebunkNewsProviderResult>
    {
        protected readonly ConcurrentQueue<Task<DebunkNewsProviderResult>> Queue;

        private IAnalyzer<DebunkNewsProviderResult> _nextAnalyzer;

        protected BaseFetcher(ConcurrentQueue<Task<DebunkNewsProviderResult>> queue)
        {
            Queue = queue ?? throw new ArgumentNullException(nameof(queue));
        }

        public IAnalyzer<DebunkNewsProviderResult> SetNext(IAnalyzer<DebunkNewsProviderResult> analyzer)
        {
            _nextAnalyzer = analyzer;
            return _nextAnalyzer;
        }

        public virtual Task<DebunkNewsProviderResult>[] AnalyzeAsync()
        {
            var nextAnalyzer = _nextAnalyzer?.AnalyzeAsync();
            return nextAnalyzer ?? Queue.ToArray();
        }
    }
}
