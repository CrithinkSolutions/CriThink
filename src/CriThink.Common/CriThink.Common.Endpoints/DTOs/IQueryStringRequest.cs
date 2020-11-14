namespace CriThink.Common.Endpoints.DTOs
{
    /// <summary>
    /// Contracts for Requests objects that can be used in query string
    /// </summary>
    public interface IQueryStringRequest
    {
        /// <summary>
        /// Returns the query string for this object
        /// </summary>
        /// <returns></returns>
        string ToQueryString();
    }
}
