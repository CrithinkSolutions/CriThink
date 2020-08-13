using System;

namespace CriThink.Server.Core.Providers
{
    public interface IProviderResult
    {
        /// <summary>
        /// Analysis type performed
        /// </summary>
        NewsAnalysisType NewsAnalysisType { get; }

        /// <summary>
        /// Indicates if the result is not available due to an unhandled error
        /// </summary>
        bool HasError { get; }

        /// <summary>
        /// Exception error, if something went wrong
        /// </summary>
        Exception Exception { get; }

        /// <summary>
        /// Error description, if something went wrong
        /// </summary>
        string ErrorDescription { get; }

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
