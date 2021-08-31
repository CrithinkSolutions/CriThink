using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CriThink.Server.Providers.Common;

namespace CriThink.Server.Providers.NewsAnalyzer.Analyzers
{
    internal class TextSentimentAnalyzer : BaseNewsAnalyzer
    {
        private readonly NewsAnalysisType _analysisType;

        public TextSentimentAnalyzer()
        {
            _analysisType = NewsAnalysisType.TextSentiment;
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
