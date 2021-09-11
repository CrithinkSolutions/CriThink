using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Domain.Entities;
using CriThink.Server.Domain.QueryResults;

namespace CriThink.Server.Domain.Repositories
{
    public interface IDebunkingNewsTriggerLogRepository : IRepository<DebunkingNewsTriggerLog>
    {
        Task<DateTime> GetLatestTimeStampAsync(CancellationToken cancellationToken = default);

        Task<IList<GetAllTriggerLogQueryResult>> GetAllTriggerLogsAsync(
            int pageSize, int pageIndex, CancellationToken cancellationToken = default);

        Task AddTriggerLogAsync(DebunkingNewsTriggerLog triggerLog, CancellationToken cancellationToken = default);
    }
}
