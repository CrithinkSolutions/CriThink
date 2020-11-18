using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CriThink.Server.Providers.Common;

namespace CriThink.Server.Providers.NewsAnalyzer.Analyzers
{
    internal class LanguageAnalyzer : BaseNewsAnalyzer
    {
        private readonly NewsAnalysisType _analysisType;

        public LanguageAnalyzer()
        {
            _analysisType = NewsAnalysisType.Ortographic;
        }

        public override Task<NewsAnalysisProviderResult>[] AnalyzeAsync()
        {
            var analysisTask = Task.Run(() =>
            {
                Debug.WriteLine("Get in LanguageAnalyzer");
                return RunAnalysis();
            });

            Queue.Enqueue(analysisTask);
            return base.AnalyzeAsync();
        }

        private NewsAnalysisProviderResult RunAnalysis()
        {
            return new NewsAnalysisProviderResult(_analysisType, ScrapedNews.RequestedUri, new NotImplementedException("Language analyzer is not implemented yet"));
        }
    }
}
