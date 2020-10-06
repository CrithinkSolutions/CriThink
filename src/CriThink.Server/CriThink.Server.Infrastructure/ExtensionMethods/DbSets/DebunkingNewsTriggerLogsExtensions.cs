using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Entities;
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
    }
}
