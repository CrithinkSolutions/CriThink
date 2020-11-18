using System;

namespace CriThink.Server.Providers.Common
{
    /// <summary>
    /// Represents an Analyzer Provider result
    /// </summary>
    public interface IAnalyzerResult : IProviderResult
    {
        /// <summary>
        /// Analysis type performed
        /// </summary>
        NewsAnalysisType NewsAnalysisType { get; }

        /// <summary>
        /// <see cref="Uri"/> analyzed
        /// </summary>
        Uri Uri { get; }

        /// <summary>
        /// Represents a score of trustworthiness of the given source. From 1 to 10
        /// </summary>
        int TrustworthinessScore { get; }
    }
}
