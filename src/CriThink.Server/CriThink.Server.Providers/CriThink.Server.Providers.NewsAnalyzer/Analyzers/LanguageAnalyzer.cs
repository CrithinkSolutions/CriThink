using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CriThink.Server.Providers.NewsAnalyzer.Analyzers
{
    internal class LanguageAnalyzer : BaseNewsAnalyzer
    {
        private readonly NewsAnalysisType _analysisType;

        public LanguageAnalyzer(NewsScraperProviderResponse scrapedNews, ConcurrentQueue<Task<NewsAnalysisProviderResponse>> queue)
            : base(scrapedNews, queue)
        {
            _analysisType = NewsAnalysisType.Ortographic;
        }

        public override Task<NewsAnalysisProviderResponse>[] AnalyzeAsync()
        {
            var analysisTask = Task.Run(() =>
            {
                Debug.WriteLine("Get in LanguageAnalyzer");
                return RunAnalysis();
            });

            Queue.Enqueue(analysisTask);
            return base.AnalyzeAsync();
        }

        private NewsAnalysisProviderResponse RunAnalysis()
        {
            throw new NotImplementedException();
        }
    }
}
