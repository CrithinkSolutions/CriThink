using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using CriThink.Server.Providers.Common;

namespace CriThink.Server.Providers.DebunkingNewsFetcher.Fetchers
{
    internal abstract class BaseFetcher : IAnalyzer<DebunkingNewsProviderResult>
    {
        private IAnalyzer<DebunkingNewsProviderResult> _nextAnalyzer;

        public ConcurrentQueue<Task<DebunkingNewsProviderResult>> Queue { get; private set; }

        public DateTime? LastFetchingTimeStamp { get; private set; }

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

        public void SetQueue(ConcurrentQueue<Task<DebunkingNewsProviderResult>> queue)
        {
            Queue = queue;
        }

        public void SetLastFetchingTimeStamp(DateTime dateTime)
        {
            LastFetchingTimeStamp = dateTime;
        }
    }
}
