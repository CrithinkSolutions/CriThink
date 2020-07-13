using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CriThink.Server.Providers.DomainAnalyzer.Providers
{
    internal class WhoIsAnalyzer : BaseAnalyzer
    {
        private readonly AnalysisType _analysisType;

        public WhoIsAnalyzer(Uri uri, ConcurrentQueue<Task<AnalysisResponse>> queue)
            : base(uri, queue)
        {
            _analysisType = AnalysisType.WhoIs;
        }

        public override Task<AnalysisResponse>[] AnalyzeAsync()
        {
            var t = Task.Run(() =>
            {
                Debug.WriteLine("Get in WhoIsAnalyzer");
                return RunAnalysis();
            });

            Queue.Enqueue(t);
            return base.AnalyzeAsync();
        }

        private AnalysisResponse RunAnalysis()
        {
            return new AnalysisResponse(_analysisType, Uri, new NotImplementedException("WhosIs analyzer is not implemented yet"));
        }
    }
}
