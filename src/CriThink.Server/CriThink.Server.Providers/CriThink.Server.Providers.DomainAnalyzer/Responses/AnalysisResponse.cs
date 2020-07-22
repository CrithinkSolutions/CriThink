using System;
// ReSharper disable once CheckNamespace

namespace CriThink.Server.Providers.DomainAnalyzer
{
    /// <summary>
    /// Result of the requested analysis
    /// </summary>
    public class AnalysisResponse
    {
        public AnalysisResponse(AnalysisType analysis, Uri uri, SourceAnalysisScore score)
        {
            Analysis = analysis;
            Uri = uri ?? throw new ArgumentNullException(nameof(uri));

            SourceAnalysisScore = score;
        }

        public AnalysisResponse(AnalysisType analysis, Uri uri, Exception ex, string errorDescription = "")
        {
            Analysis = analysis;
            Uri = uri ?? throw new ArgumentNullException(nameof(uri));

            Exception = ex ?? throw new ArgumentNullException(nameof(analysis));
            ErrorDescription = string.IsNullOrWhiteSpace(errorDescription) ? ex.Message : errorDescription;
            SourceAnalysisScore = SourceAnalysisScore.Unknown;
        }

        public AnalysisType Analysis { get; }

        public SourceAnalysisScore SourceAnalysisScore { get; }

        public bool HasError => Exception != null;

        public Exception Exception { get; }

        public string ErrorDescription { get; }

        public Uri Uri { get; }
    }

    /// <summary>
    /// List of differnet analysis types available
    /// </summary>
    public enum AnalysisType
    {
        /// <summary>
        /// Check if the given Uri has HTTPS support
        /// </summary>
        HTTPS,

        /// <summary>
        /// Check domain information
        /// </summary>
        WhoIs
    }
}
