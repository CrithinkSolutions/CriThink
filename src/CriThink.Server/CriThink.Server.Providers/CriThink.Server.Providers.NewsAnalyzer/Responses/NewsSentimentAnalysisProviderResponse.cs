// ReSharper disable CheckNamespace

namespace CriThink.Server.Providers.NewsAnalyzer
{
    /// <summary>
    /// The text sentiment analysis provider response
    /// </summary>
    public class NewsSentimentAnalysisProviderResponse
    {
        public NewsSentimentAnalysisProviderResponse(string text, double sentiment, double positiveScore, double neutralScore, double negativeScore)
        {
            Text = text;
            Sentiment = sentiment;
            PositiveScore = positiveScore;
            NeutralScore = neutralScore;
            NegativeScore = negativeScore;
        }

        public string Text { get; }

        public double Sentiment { get; }

        public double PositiveScore { get; }

        public double NeutralScore { get; }

        public double NegativeScore { get; }
    }
}
