using System;
using CriThink.Server.Providers.Common;

// ReSharper disable once CheckNamespace

namespace CriThink.Server.Providers.DomainAnalyzer
{
    /// <summary>
    /// Result of the requested analysis
    /// </summary>
    public class DomainAnalysisProviderResult : IAnalyzerResult
    {
        public DomainAnalysisProviderResult(NewsAnalysisType analysis, Uri uri, int trustworthinessScore)
        {
            NewsAnalysisType = analysis;
            Uri = uri ?? throw new ArgumentNullException(nameof(uri));

            TrustworthinessScore = trustworthinessScore;
        }

        public DomainAnalysisProviderResult(NewsAnalysisType analysis, Uri uri, Exception ex, string errorDescription = "")
        {
            NewsAnalysisType = analysis;
            Uri = uri ?? throw new ArgumentNullException(nameof(uri));

            Exception = ex ?? throw new ArgumentNullException(nameof(analysis));
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
