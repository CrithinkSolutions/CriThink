using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using CriThink.Server.Core.Providers;

namespace CriThink.Server.Providers.DomainAnalyzer.Analyzers
{
    internal abstract class BaseDomainAnalyzer : IAnalyzer<DomainAnalysisProviderResult>
    {
        protected readonly ConcurrentQueue<Task<DomainAnalysisProviderResult>> Queue;

        private IAnalyzer<DomainAnalysisProviderResult> _nextAnalyzer;

        protected BaseDomainAnalyzer(Uri uri, ConcurrentQueue<Task<DomainAnalysisProviderResult>> queue)
        {
            Uri = uri;
            Queue = queue;
        }

        protected Uri Uri { get; }

        public IAnalyzer<DomainAnalysisProviderResult> SetNext(IAnalyzer<DomainAnalysisProviderResult> analyzer)
        {
            _nextAnalyzer = analyzer;
            return _nextAnalyzer;
        }

        public virtual Task<DomainAnalysisProviderResult>[] AnalyzeAsync()
        {
            var nextAnalyzer = _nextAnalyzer?.AnalyzeAsync();
            return nextAnalyzer ?? Queue.ToArray();
        }
    }
}
