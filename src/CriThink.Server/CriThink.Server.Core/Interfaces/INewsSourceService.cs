using System;
using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.NewsSource;

namespace CriThink.Server.Core.Interfaces
{
    public interface INewsSourceService
    {
        /// <summary>
        /// Add the given source to the black or whitelist
        /// </summary>
        /// <param name="request">Source with the classification</param>
        /// <returns>A task</returns>
        Task AddSourceAsync(NewsSourceAddRequest request);

        /// <summary>
        /// Remove a source from the blacklist
        /// </summary>
        /// <param name="uri"><see cref="Uri"/> to analyze</param>
        /// <returns>A task</returns>
        Task RemoveNewsSourceAsync(Uri uri);

        /// <summary>
        /// Search the given source in the black and whitelist
        /// </summary>
        /// <param name="uri"><see cref="Uri"/> to analyze</param>
        /// <returns></returns>
        Task<NewsSourceSearchResponse> SearchNewsSourceAsync(Uri uri);

        /// <summary>
        /// Search the given source in the black and whitelist and send an alert if the source is unknown
        /// </summary>
        /// <param name="uri"><see cref="Uri"/> to analyze</param>
        /// <returns></returns>
        Task<NewsSourceSearchResponse> SearchNewsSourceWithAlertAsync(Uri uri);

        /// <summary>
        /// Get all the news sources stored. Result can be filtered
        /// </summary>
        /// <param name="request">Pagination and filter</param>
        /// <returns>All the news sources</returns>
        Task<NewsSourceGetAllResponse> GetAllNewsSourcesAsync(NewsSourceGetAllRequest request);
    }
}
