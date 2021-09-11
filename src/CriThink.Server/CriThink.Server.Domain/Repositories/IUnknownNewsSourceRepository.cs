using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Domain.Entities;
using CriThink.Server.Domain.QueryResults;

namespace CriThink.Server.Domain.Repositories
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
