namespace CriThink.Common.Endpoints.DTOs.NewsSource.Requests
{
    /// <summary>
    /// Filter options when getting all news sources
    /// </summary>
    public enum NewsSourceGetAllFilterRequest
    {
        /// <summary>
        /// No filter applied
        /// </summary>
        None,

        /// <summary>
        /// Get only white listed news sources
        /// </summary>
        Good,

        /// <summary>
        /// Get only black listed news sources
        /// </summary>
        Bad
    }
}
