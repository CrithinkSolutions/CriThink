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
using Microsoft.EntityFrameworkCore;
using Npgsql;

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

        public IUnitOfWork UnitOfWork => _dbContext;

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

        public async Task<UnknownNewsSource> GetUnknownNewsSourceByUriAsync(
            string newsLink,
            CancellationToken cancellationToken = default)
        {
            var id = await _dbContext.UnknownNewsSources.GetUnknownSourceByUriAsync(
                newsLink,
                UnknownNewsSourceProjection.GetUnknownNewsSource,
                cancellationToken);

            return id;
        }

        public async Task AddUnknownNewsSourceAsync(
            string newsLink)
        {
            var sqlQuery = "INSERT INTO unknown_news_sources\n" +
                           "(id, uri, first_requested_at, request_count, authenticity)\n" +
                           "VALUES\n" +
                           "({0}, {1}, {2}, {3}, {4})\n" +
                           "ON CONFLICT (uri)\n" +
                           "DO UPDATE\n" +
                           "SET\n" +
                           "request_count = (unknown_news_sources.request_count + 1);";

            var id = new NpgsqlParameter("id", Guid.NewGuid());
            var uri = new NpgsqlParameter("uri", newsLink);
            var firstRequestedAt = new NpgsqlParameter("first_requested_at", DateTime.UtcNow);
            var requestCount = new NpgsqlParameter("request_count", 1);
            var authenticity = new NpgsqlParameter("authenticity", NewsSourceAuthenticity.Unknown.ToString());

            await _dbContext.Database.ExecuteSqlRawAsync(sqlQuery,
                    id,
                    uri,
                    firstRequestedAt,
                    requestCount,
                    authenticity);
        }
    }
}
