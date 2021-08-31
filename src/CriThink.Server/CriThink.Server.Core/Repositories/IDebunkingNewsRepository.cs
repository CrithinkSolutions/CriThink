using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.QueryResults;

namespace CriThink.Server.Core.Repositories
{
    public interface IDebunkingNewsRepository : IRepository<DebunkingNews>
    {
        Task<IList<GetAllDebunkingNewsQueryResult>> GetAllDebunkingNewsAsync(
            int pageSize,
            int pageIndex,
            string languageFilter = null);

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
