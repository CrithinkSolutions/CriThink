using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CriThink.Server.Providers.Common;
using CriThink.Server.Providers.NewsAnalyzer.Singletons;

namespace CriThink.Server.Providers.NewsAnalyzer.Analyzers
{
    internal class TextSentimentAnalyzer : BaseNewsAnalyzer
    {
        private readonly NewsAnalysisType _analysisType;
        private readonly NewsAnalyticsClient _newsAnalyticsClient;

        public TextSentimentAnalyzer(NewsAnalyticsClient newsAnalyticsClient)
        {
            _analysisType = NewsAnalysisType.TextSentiment;
            _newsAnalyticsClient = newsAnalyticsClient;
        }

        public override Task<NewsAnalysisProviderResult>[] AnalyzeAsync()
        {
            var analysisTask = Task.Run(() =>
            {
                Debug.WriteLine("Get in TextSentimentAnalyzer");
                return RunAnalysis();
            });

            Queue.Enqueue(analysisTask);
            return base.AnalyzeAsync();
        }

        private NewsAnalysisProviderResult RunAnalysis()
        {
            return new NewsAnalysisProviderResult(_analysisType, ScrapedNews.RequestedUri, new NotImplementedException("Text analyzer is not implemented yet"));
        }
    }
}
