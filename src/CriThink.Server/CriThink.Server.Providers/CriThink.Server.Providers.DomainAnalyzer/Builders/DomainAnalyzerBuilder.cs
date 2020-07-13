using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using CriThink.Server.Providers.DomainAnalyzer.Providers;

namespace CriThink.Server.Providers.DomainAnalyzer.Builders
{
    public class DomainAnalyzerBuilder
    {
        private bool _isWhoIsEnabled;
        private bool _isHttpsEnabled;
        private Uri _uri;

        private IAnalyzer _analyzer;
        private readonly ConcurrentQueue<Task<AnalysisResponse>> _queue;

        public DomainAnalyzerBuilder()
        {
            _queue = new ConcurrentQueue<Task<AnalysisResponse>>();
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

        internal IAnalyzer BuildAnalyzers()
        {
            _queue.Clear();

            if (_isHttpsEnabled)
                AddAnalyzer(new HttpsAnalyzer(_uri, _queue));

            if (_isWhoIsEnabled)
                AddAnalyzer(new WhoIsAnalyzer(_uri, _queue));

            return _analyzer;
        }

        private void AddAnalyzer(IAnalyzer analyzer)
        {
            if (_analyzer == null)
                _analyzer = analyzer;
            else
                _analyzer.SetNext(analyzer);
        }
    }
}
