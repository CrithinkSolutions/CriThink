using System;

// ReSharper disable CheckNamespace

namespace CriThink.Server.Providers.NewsAnalyzer
{
    public class NewsAnalysisProviderResponse
    {
        public NewsAnalysisProviderResponse(NewsAnalysisType analysis, Uri uri, NewsAnalysisScore score)
        {
            Analysis = analysis;
            Uri = uri ?? throw new ArgumentNullException(nameof(uri));

            NewsAnalysisScore = score;
        }

        public NewsAnalysisProviderResponse(NewsAnalysisType analysis, Uri uri, Exception ex, string errorDescription = "")
        {
            Analysis = analysis;
            Uri = uri;

            Exception = ex ?? throw new ArgumentNullException(nameof(analysis));
            ErrorDescription = string.IsNullOrWhiteSpace(errorDescription) ? ex.Message : errorDescription;
            NewsAnalysisScore = NewsAnalysisScore.Unknown;
        }

        public NewsAnalysisType Analysis { get; }

        public NewsAnalysisScore NewsAnalysisScore { get; }

        public bool HasError => Exception != null;

        public Exception Exception { get; }

        public string ErrorDescription { get; }

        public Uri Uri { get; }
    }

    /// <summary>
    /// List of differnet analysis types available
    /// </summary>
    public enum NewsAnalysisType
    {
        /// <summary>
        /// News body text ortographic
        /// </summary>
        Ortographic,

        /// <summary>
        /// Text sentiment analysis
        /// </summary>
        TextSentiment
    }
}
