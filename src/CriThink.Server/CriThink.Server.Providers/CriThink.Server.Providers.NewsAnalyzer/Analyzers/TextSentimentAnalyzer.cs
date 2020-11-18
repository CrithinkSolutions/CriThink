using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Azure.AI.TextAnalytics;
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
            if (ScrapedNews == null)
                return new NewsAnalysisProviderResult(_analysisType, null, new InvalidOperationException("The given URL is null"));

            DocumentSentiment documentSentiment = _newsAnalyticsClient.Instance.AnalyzeSentiment(ScrapedNews.NewsBody);

            var negativeAverage = 0d;

            foreach (var sentence in documentSentiment.Sentences)
            {
                Debug.WriteLine($"\tText: \"{sentence.Text}\"");
                Debug.WriteLine($"\tSentence sentiment: {sentence.Sentiment}");
                Debug.WriteLine($"\tPositive score: {sentence.ConfidenceScores.Positive:0.00}");
                Debug.WriteLine($"\tNegative score: {sentence.ConfidenceScores.Negative:0.00}");
                Debug.WriteLine($"\tNeutral score: {sentence.ConfidenceScores.Neutral:0.00}\n");

                negativeAverage += sentence.ConfidenceScores.Negative;
            }

            negativeAverage /= documentSentiment.Sentences.Count;

            var score = (int) Math.Round(negativeAverage * 10, MidpointRounding.ToEven);

            return new NewsAnalysisProviderResult(_analysisType, ScrapedNews.RequestedUri, score);
        }
    }
}
