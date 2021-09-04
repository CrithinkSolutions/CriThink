using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.QueryResults;

namespace CriThink.Server.Core.Repositories
{
    public interface IDebunkingNewsTriggerLogRepository : IRepository<DebunkingNewsTriggerLog>
    {
        Task<DateTime> GetLatestTimeStampAsync(CancellationToken cancellationToken = default);

        Task<IList<GetAllTriggerLogQueryResult>> GetAllTriggerLogsAsync(
            int pageSize, int pageIndex, CancellationToken cancellationToken = default);

        Task AddTriggerLogAsync(DebunkingNewsTriggerLog triggerLog, CancellationToken cancellationToken = default);
    }
}
