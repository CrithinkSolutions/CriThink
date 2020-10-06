namespace CriThink.Server.Core.Providers
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
