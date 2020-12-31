using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.Admin;

namespace CriThink.Server.Core.Interfaces
{
    public interface IDebunkingNewsService
    {
        /// <summary>
        /// Update the debunk news repository
        /// </summary>
        /// <returns></returns>
        Task UpdateRepositoryAsync();

        /// <summary>
        /// Add the given debunking news to the repository
        /// </summary>
        /// <param name="request">Debunking news</param>
        /// <returns></returns>
        Task AddDebunkingNewsAsync(DebunkingNewsAddRequest request);

        /// <summary>
        /// Delete the debunking news with the associated id
        /// </summary>
        /// <param name="request">Debunking news id</param>
        /// <returns></returns>
        Task DeleteDebunkingNewsAsync(SimpleDebunkingNewsRequest request);

        /// <summary>
        /// Update the debunking news
        /// </summary>
        /// <param name="request">The new list of keyworks</param>
        /// <returns></returns>
        Task UpdateDebunkingNewsAsync(DebunkingNewsUpdateRequest request);

        /// <summary>
        /// Get all the debunking news
        /// </summary>
        /// <param name="request">Page index and debunking news per page</param>
        /// <returns></returns>
        Task<DebunkingNewsGetAllResponse> GetAllDebunkingNewsAsync(DebunkingNewsGetAllRequest request);

        /// <summary>
        /// Get the specified debunking news
        /// </summary>
        /// <param name="request">Debunking news id</param>
        /// <returns></returns>
        Task<DebunkingNewsGetDetailsResponse> GetDebunkingNewsAsync(DebunkingNewsGetRequest request);

        /// <summary>
        /// Get all trigger logs paginated
        /// </summary>
        /// <param name="request">Pagination</param>
        /// <returns>List of trigger logs</returns>
        Task<TriggerLogsGetAllResponse> GetAllTriggerLogsAsync(TriggerLogsGetAllRequest request);
    }
}
