using System;
using CriThink.Server.Providers.Common;

// ReSharper disable once CheckNamespace

namespace CriThink.Server.Providers.NewsAnalyzer
{
    /// <summary>
    /// Result of the requested analysis
    /// </summary>
    public class NewsAnalysisProviderResult : IAnalyzerResult
    {
        public NewsAnalysisProviderResult(NewsAnalysisType analysis, Uri uri, int trustworthinessScore)
        {
            NewsAnalysisType = analysis;
            Uri = uri ?? throw new ArgumentNullException(nameof(uri));

            TrustworthinessScore = trustworthinessScore;
        }

        public NewsAnalysisProviderResult(NewsAnalysisType analysis, Uri uri, Exception ex, string errorDescription = "")
        {
            NewsAnalysisType = analysis;
            Uri = uri;

            Exception = ex ?? throw new ArgumentNullException(nameof(ex));
            ErrorDescription = string.IsNullOrWhiteSpace(errorDescription) ? ex.Message : errorDescription;
        }

        public NewsAnalysisType NewsAnalysisType { get; }

        public Exception Exception { get; }

        public bool HasError => Exception != null;

        public string ErrorDescription { get; }

        public Uri Uri { get; }

        public int TrustworthinessScore { get; }
    }
}
