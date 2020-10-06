using System;

namespace CriThink.Server.Core.Providers
{
    /// <summary>
    /// Represents a Provider result
    /// </summary>
    public interface IProviderResult
    {
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
    }
}
