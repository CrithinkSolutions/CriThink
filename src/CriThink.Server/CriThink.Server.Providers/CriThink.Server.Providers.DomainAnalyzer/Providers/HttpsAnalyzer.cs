using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CriThink.Server.Providers.DomainAnalyzer.Providers
{
    internal class HttpsAnalyzer : BaseAnalyzer
    {
        private readonly AnalysisType _analysisType;

        public HttpsAnalyzer(Uri uri, ConcurrentQueue<Task<AnalysisResponse>> queue)
            : base(uri, queue)
        {
            _analysisType = AnalysisType.HTTPS;
        }

        public override Task<AnalysisResponse>[] AnalyzeAsync()
        {
            var analysisTask = Task.Run(() =>
            {
                Debug.WriteLine("Get in HttpsAnalyzer");
                return RunAnalysis();
            });

            Queue.Enqueue(analysisTask);
            return base.AnalyzeAsync();
        }

        private AnalysisResponse RunAnalysis()
        {
            if (Uri == null)
                return new AnalysisResponse(_analysisType, Uri, new InvalidOperationException("The given URL is null"));

            var score = Uri.Scheme == Uri.UriSchemeHttps ? SourceAnalysisScore.Trust : SourceAnalysisScore.Untrusted;
            return new AnalysisResponse(_analysisType, Uri, score);
        }
    }
}
