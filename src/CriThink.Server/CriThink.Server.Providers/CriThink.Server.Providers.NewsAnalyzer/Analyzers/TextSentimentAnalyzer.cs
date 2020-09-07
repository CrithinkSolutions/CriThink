using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;
using Azure;
using Azure.AI.TextAnalytics;
using CriThink.Server.Core.Providers;

namespace CriThink.Server.Providers.NewsAnalyzer.Analyzers
{
    internal class TextSentimentAnalyzer : BaseNewsAnalyzer
    {
        internal static string AzureEndpoint;
        internal static string AzureCredentials;

        private readonly NewsAnalysisType _analysisType;
        private readonly TextAnalyticsClient _analyticsClient;

        public TextSentimentAnalyzer(NewsScraperProviderResponse scrapedNews, ConcurrentQueue<Task<NewsAnalysisProviderResult>> queue)
            : base(scrapedNews, queue)
        {
            _analysisType = NewsAnalysisType.TextSentiment;
            _analyticsClient = new TextAnalyticsClient(new Uri(AzureEndpoint), new AzureKeyCredential(AzureCredentials));
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

            DocumentSentiment documentSentiment = _analyticsClient.AnalyzeSentiment(ScrapedNews.NewsBody);

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
