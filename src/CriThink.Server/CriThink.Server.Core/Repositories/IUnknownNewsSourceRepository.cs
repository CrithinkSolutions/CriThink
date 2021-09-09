using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.QueryResults;

namespace CriThink.Server.Core.Repositories
{
    public interface IUnknownNewsSourceRepository : IRepository<UnknownNewsSource>
    {
        Task<List<GetAllUnknownSourcesQueryResult>> GetAllUnknownSourcesAsync(
            int pageSize,
            int pageIndex,
            CancellationToken cancellationToken = default);

        Task<UnknownNewsSource> GetUnknownNewsSourceByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<UnknownNewsSource> GetUnknownNewsSourceByUriAsync(
            string newsLink,
            CancellationToken cancellationToken = default);

        Task<IList<GetAllSubscribedUsersQueryResult>> GetAllSubscribedUsersAsync(
            Guid unknownNewsId,
            int pageSize,
            int pageIndex,
            CancellationToken cancellationToken = default);

        Task AddUnknownNewsSourceAsync(
            string newsLink);
    }
}
