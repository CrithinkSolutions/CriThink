using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace CriThink.Server.Infrastructure.ExtensionMethods.DbSets
{
    internal static class DebunkingNewsTriggerLogsExtensions
    {
        /// <summary>
        /// Get the most recent successfull entry's timestamp
        /// </summary>
        /// <param name="dbSet">This <see cref="DbSet{TEntity}"/></param>
        /// <param name="projection">Projection applied to Select query</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns></returns>
        internal static Task<DateTime> GetMostRecentSuccessfullDateAsync(this DbSet<DebunkingNewsTriggerLog> dbSet, Expression<Func<DebunkingNewsTriggerLog, DateTime>> projection, CancellationToken cancellationToken = default)
        {
            return dbSet
                .Where(log => log.IsSuccessful)
                .OrderBy(log => log.TimeStamp)
                .Select(projection)
                .FirstOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// Returns paginated debunking news trigger logs
        /// </summary>
        /// <param name="dbSet">This <see cref="DbSet{TEntity}"/></param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="projection">Projection applied to Select query</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns></returns>
        internal static Task<List<GetAllTriggerLogQueryResponse>> GetAllDebunkingNewsTriggerLogsAsync(
            this DbSet<DebunkingNewsTriggerLog> dbSet,
            int pageSize,
            int pageIndex,
            Expression<Func<DebunkingNewsTriggerLog, GetAllTriggerLogQueryResponse>> projection,
            CancellationToken cancellationToken = default)
        {
            return dbSet
                .Skip(pageSize * (pageIndex - 1))
                .Take(pageSize)
                .Select(projection)
                .ToListAsync(cancellationToken);
        }
    }
}
