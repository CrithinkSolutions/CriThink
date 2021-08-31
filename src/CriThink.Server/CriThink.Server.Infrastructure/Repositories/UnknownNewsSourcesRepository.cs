using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.QueryResults;
using CriThink.Server.Core.Repositories;
using CriThink.Server.Infrastructure.Data;
using CriThink.Server.Infrastructure.ExtensionMethods.DbSets;
using CriThink.Server.Infrastructure.Projections;

namespace CriThink.Server.Infrastructure.Repositories
{
    internal class UnknownNewsSourcesRepository : IUnknownNewsSourcesRepository
    {
        private readonly CriThinkDbContext _dbContext;

        public UnknownNewsSourcesRepository(
            CriThinkDbContext dbContext)
        {
            _dbContext = dbContext ??
                throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IList<GetAllSubscribedUsersQueryResult>> GetAllSubscribedUsersAsync(
            Guid unknownNewsId,
            int pageSize,
            int pageIndex,
            CancellationToken cancellationToken = default)
        {
            var users = await _dbContext.UnknownNewsSourceNotificationRequests
                .GetAllSubscribedUsersForUnknownNewsSourceId(
                    unknownNewsId,
                    pageSize,
                    pageIndex,
                    UnknownSourceNotificationRequestProjection.GetAll,
                    cancellationToken);

            return users;
        }

        public async Task<List<GetAllUnknownSourcesQueryResult>> GetAllUnknownSourcesAsync(
            int pageSize,
            int pageIndex,
            CancellationToken cancellationToken = default)
        {
            var unknownNewsSourceCollection = await _dbContext.UnknownNewsSources
                    .GetAllUnknownSourceAsync(pageSize, pageIndex, UnknownNewsSourceProjection.GetAll, cancellationToken);

            return unknownNewsSourceCollection;
        }

        public async Task<UnknownNewsSource> GetUnknownNewsSourceByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var response = await _dbContext.UnknownNewsSources.GetUnknownNewsSourceByIdAsync(
                id,
                UnknownNewsSourceProjection.GetUnknownNewsSource, cancellationToken);

            return response;
        }

        public async Task<Guid> GetUnknownNewsSourceByIdAsync(string newsLink, CancellationToken cancellationToken = default)
        {
            var id = await _dbContext.UnknownNewsSources.GetUnknownSourceIdByUriAsync(
                newsLink,
                UnknownNewsSourceProjection.GetId,
                cancellationToken);

            return id;
        }
    }
}
