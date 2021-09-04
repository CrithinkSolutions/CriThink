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
    internal class DebunkingNewsTriggerLogRepository : IDebunkingNewsTriggerLogRepository
    {
        private readonly CriThinkDbContext _dbContext;

        public DebunkingNewsTriggerLogRepository(
            CriThinkDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IUnitOfWork UnitOfWork => _dbContext;

        public async Task AddTriggerLogAsync(
            DebunkingNewsTriggerLog triggerLog,
            CancellationToken cancellationToken = default)
        {
            await _dbContext.DebunkingNewsTriggerLogs.AddAsync(triggerLog, cancellationToken);
        }

        public async Task<IList<GetAllTriggerLogQueryResult>> GetAllTriggerLogsAsync(
            int pageSize,
            int pageIndex,
            CancellationToken cancellationToken = default)
        {
            var triggerLogs = await _dbContext.DebunkingNewsTriggerLogs
                    .GetAllDebunkingNewsTriggerLogsAsync(
                pageSize,
                pageIndex,
                DebunkingNewsTriggerLogsProjection.GetAll,
                cancellationToken);

            return triggerLogs;
        }

        public async Task<DateTime> GetLatestTimeStampAsync(
            CancellationToken cancellationToken = default)
        {
            var latestEntry = await _dbContext.DebunkingNewsTriggerLogs
                    .GetMostRecentSuccessfullDateAsync(
                DebunkingNewsTriggerLogsProjection.GetTimeStamp,
                cancellationToken);

            return latestEntry;
        }
    }
}
