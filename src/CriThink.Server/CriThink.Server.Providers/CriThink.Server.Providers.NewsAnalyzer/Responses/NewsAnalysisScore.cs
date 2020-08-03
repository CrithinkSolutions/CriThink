// ReSharper disable once CheckNamespace

namespace CriThink.Server.Providers.NewsAnalyzer
{
    public enum NewsAnalysisScore
    {
        /// <summary>
        /// Can't give a score to the given news
        /// </summary>
        Unknown,

        /// <summary>
        /// The given news is trustworthy
        /// </summary>
        Trustworthy,

        /// <summary>
        /// The given news is not trusted
        /// </summary>
        NotTrusted
    }
}
