using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using CriThink.Server.Application.Administration.ViewModels;
using CriThink.Server.Core.QueryResults;

namespace CriThink.Server.Application.Queries
{
    public interface INewsSourceQueries
    {
        /// <summary>
        /// Returns questions for an article
        /// </summary>
        /// <returns></returns>
        Task<NewsSourceGetAllQuestionsResponse> GetGeneralQuestionsAsync();

        /// <summary>
        /// Get all news sources with pagination support. Results
        /// can be filtered by goodness
        /// </summary>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="filter">Goodness filter</param>
        /// <returns></returns>
        IList<GetAllNewsSourceQueryResult> GetAllNewsSources(int pageSize, int pageIndex, NewsSourceAuthenticityFilter filter);

        /// <summary>
        /// Get the news source by its name
        /// </summary>
        /// <param name="name">News source name</param>
        /// <returns></returns>
        Task<NewsSourceSearchResponse> GetNewsSourceByNameAsync(string name);

        /// <summary>
        /// Get the unknown source by id
        /// </summary>
        /// <param name="id">Unknown source id</param>
        /// <returns></returns>
        Task<UnknownNewsSourceResponse> GetUnknownNewsSourceByIdAsync(Guid id);

        /// <summary>
        /// Get all the unknown news sources with pagination
        /// support.
        /// </summary>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageIndex">Page index</param>
        /// <returns></returns>
        Task<UnknownNewsSourceGetAllViewModel> GetAllUnknownNewsSourcesAsync(int pageSize, int pageIndex);
    }
}
