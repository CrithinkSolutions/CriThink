using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using CriThink.Common.Endpoints.DTOs.NewsSource.Requests;

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
        Task RemoveBadSourceAsync(Uri uri);

        /// <summary>
        /// Remove a source from the whitelist
        /// </summary>
        /// <param name="uri"><see cref="Uri"/> to analyze</param>
        /// <returns>A task</returns>
        Task RemoveGoodNewsSourceAsync(Uri uri);

        /// <summary>
        /// Search the given source in the black and whitelist
        /// </summary>
        /// <param name="uri"><see cref="Uri"/> to analyze</param>
        /// <returns></returns>
        Task<NewsSourceSearchResponse> SearchNewsSourceAsync(Uri uri);

        /// <summary>
        /// Get all the news sources stored. Result can be filtered
        /// </summary>
        /// <param name="request">Optional filter</param>
        /// <returns>All the news sources</returns>
        Task<IList<NewsSourceGetAllResponse>> GetAllNewsSourcesAsync(NewsSourceGetAllFilterRequest request);
    }
}
