using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;
using CriThink.Server.Core.Providers;

namespace CriThink.Server.Providers.DomainAnalyzer.Analyzers
{
    internal class WhoIsAnalyzer : BaseDomainAnalyzer
    {
        private readonly NewsAnalysisType _analysisType;

        public WhoIsAnalyzer(Uri uri, ConcurrentQueue<Task<DomainAnalysisProviderResult>> queue)
            : base(uri, queue)
        {
            _analysisType = NewsAnalysisType.WhoIs;
        }

        public override Task<DomainAnalysisProviderResult>[] AnalyzeAsync()
        {
            var whoIsTask = Task.Run(() =>
            {
                Debug.WriteLine("Get in WhoIsAnalyzer");
                return RunAnalysis();
            });

            Queue.Enqueue(whoIsTask);
            return base.AnalyzeAsync();
        }

        private DomainAnalysisProviderResult RunAnalysis()
        {
            if (Uri == null)
                return new DomainAnalysisProviderResult(_analysisType, Uri, new InvalidOperationException("The given URL is null"));

            return new DomainAnalysisProviderResult(_analysisType, Uri, new NotImplementedException("WhosIs analyzer is not implemented yet"));
        }
    }
}
