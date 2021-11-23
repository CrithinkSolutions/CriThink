using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.DebunkingNews;
using CriThink.Common.Endpoints.DTOs.NewsSource;

namespace CriThink.Server.Application.Queries
{
    public interface IDebunkingNewsQueries
    {
        /// <summary>
        /// Get all the debunking news
        /// </summary>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="languageFilter">(Optional) Language filter (iso name)</param>
        /// <returns></returns>
        Task<DebunkingNewsGetAllResponse> GetAllDebunkingNewsAsync(
            int pageSize,
            int pageIndex,
            string languageFilter = null);

        /// <summary>
        /// Get the specified debunking news
        /// </summary>
        /// <param name="request">Debunking news id</param>
        /// <returns></returns>
        Task<DebunkingNewsGetDetailsResponse> GetDebunkingNewsByIdAsync(Guid id);

        /// <summary>
        /// Search debunking news by text
        /// </summary>
        /// <param name="query">Keyword</param>
        /// <returns></returns>
        Task<IEnumerable<NewsSourceRelatedDebunkingNewsResponse>> SearchByTextAsync(string query);
    }
}
