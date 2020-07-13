// ReSharper disable once CheckNamespace

namespace CriThink.Server.Providers.DomainAnalyzer
{
    public enum SourceAnalysisScore
    {
        /// <summary>
        /// Can't give a score to the given source
        /// </summary>
        Unknown,

        /// <summary>
        /// The given source is fully trusted
        /// </summary>
        Trust,

        /// <summary>
        /// The given source is not trusted
        /// </summary>
        Untrusted,

        /// <summary>
        /// The given source might not be reliable
        /// </summary>
        Warning
    }
}
