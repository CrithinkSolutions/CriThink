using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Domain.Entities;
using CriThink.Server.Domain.QueryResults;

namespace CriThink.Server.Domain.Repositories
{
    public interface IDebunkingNewsRepository : IRepository<DebunkingNews>
    {
        Task<IList<GetAllDebunkingNewsQueryResult>> GetAllDebunkingNewsAsync(
            int pageSize,
            int pageIndex,
            string languageFilter,
            string countryFilter);

        Task<DebunkingNews> GetDebunkingNewsByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task AddDebunkingNewsAsync(
            DebunkingNews debunkingNews,
            CancellationToken cancellationToken = default);

        Task RemoveDebunkingNewsByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task UpdateDebunkingNewsAsync(
            Guid id,
            string title,
            string caption,
            string link,
            IList<string> keywords,
            string imageLink,
            CancellationToken cancellationToken = default);

        Task<IList<GetAllDebunkingNewsByKeywordsQueryResult>> GetAllDebunkingNewsByKeywordsAsync(
            IEnumerable<string> keywords,
            CancellationToken cancellationToken = default);
    }
}
