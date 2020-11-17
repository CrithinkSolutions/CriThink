using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;
using CriThink.Server.Providers.Common;

namespace CriThink.Server.Providers.DomainAnalyzer.Analyzers
{
    internal class HttpsAnalyzer : BaseDomainAnalyzer
    {
        private readonly NewsAnalysisType _analysisType;

        public HttpsAnalyzer(Uri uri, ConcurrentQueue<Task<DomainAnalysisProviderResult>> queue)
            : base(uri, queue)
        {
            _analysisType = NewsAnalysisType.HTTPS;
        }

        public override Task<DomainAnalysisProviderResult>[] AnalyzeAsync()
        {
            var analysisTask = Task.Run(() =>
            {
                Debug.WriteLine("Get in HttpsAnalyzer");
                return RunAnalysis();
            });

            Queue.Enqueue(analysisTask);
            return base.AnalyzeAsync();
        }

        private DomainAnalysisProviderResult RunAnalysis()
        {
            if (Uri == null)
                return new DomainAnalysisProviderResult(_analysisType, Uri, new InvalidOperationException("The given URL is null"));

            var score = Uri.Scheme == Uri.UriSchemeHttps ? 10 : 1;
            return new DomainAnalysisProviderResult(_analysisType, Uri, score);
        }
    }
}
