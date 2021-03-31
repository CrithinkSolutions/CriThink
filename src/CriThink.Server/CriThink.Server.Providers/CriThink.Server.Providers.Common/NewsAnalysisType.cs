namespace CriThink.Server.Providers.Common
{
    /// <summary>
    /// Enum representing the different types of analysis
    /// </summary>
    public enum NewsAnalysisType
    {
        /// <summary>
        /// News body text ortographic
        /// </summary>
        Ortographic,

        /// <summary>
        /// News body sentiment analysis
        /// </summary>
        TextSentiment,

        /// <summary>
        /// News title and content keywords
        /// </summary>
        Keywords,
    }
}
