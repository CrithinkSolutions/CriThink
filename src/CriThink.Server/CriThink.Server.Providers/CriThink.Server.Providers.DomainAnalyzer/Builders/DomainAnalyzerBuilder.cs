using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using CriThink.Server.Core.Providers;
using CriThink.Server.Providers.DomainAnalyzer.Analyzers;

namespace CriThink.Server.Providers.DomainAnalyzer.Builders
{
    public class DomainAnalyzerBuilder
    {
        private readonly ConcurrentQueue<Task<DomainAnalysisProviderResult>> _queue;

        private bool _isWhoIsEnabled;
        private bool _isHttpsEnabled;
        private Uri _uri;

        private IAnalyzer<DomainAnalysisProviderResult> _analyzer;

        public DomainAnalyzerBuilder()
        {
            _queue = new ConcurrentQueue<Task<DomainAnalysisProviderResult>>();
        }

        public DomainAnalyzerBuilder SetUri(Uri uri)
        {
            _uri = uri ?? throw new ArgumentNullException(nameof(uri));
            return this;
        }

        public DomainAnalyzerBuilder EnableHttpsAnalysis(bool enabled = true)
        {
            _isHttpsEnabled = enabled;
            return this;
        }

        public DomainAnalyzerBuilder EnableDomainAnalysis(bool enabled = true)
        {
            _isWhoIsEnabled = enabled;
            return this;
        }

        internal IAnalyzer<DomainAnalysisProviderResult> BuildAnalyzers()
        {
            _queue.Clear();

            if (_isHttpsEnabled)
                AddAnalyzer(new HttpsAnalyzer(_uri, _queue));

            if (_isWhoIsEnabled)
                AddAnalyzer(new WhoIsAnalyzer(_uri, _queue));

            return _analyzer;
        }

        private void AddAnalyzer(IAnalyzer<DomainAnalysisProviderResult> analyzer)
        {
            if (_analyzer == null)
                _analyzer = analyzer;
            else
                _analyzer.SetNext(analyzer);
        }
    }
}
