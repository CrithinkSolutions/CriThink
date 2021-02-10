using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CriThink.Server.Infrastructure.ExtensionMethods.DbSets
{
    internal static class DemoNewsExtensions
    {
        /// <summary>
        /// Get all the demo news
        /// </summary>
        /// <param name="dbSet">This <see cref="DbSet{TEntity}"/></param>
        /// <param name="projection">Projection applied to Select query</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Awaitable task</returns>
        internal static Task<List<DemoNews>> GetAllDemoNewsAsync(this DbSet<DemoNews> dbSet, Expression<Func<DemoNews, DemoNews>> projection, CancellationToken cancellationToken = default)
        {
            return dbSet
                .Select(projection)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Get demo news with the given id
        /// </summary>
        /// <param name="dbSet">This <see cref="DbSet{TEntity}"/></param>
        /// <param name="demoNewsId">Question id to search</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns></returns>
        internal static ValueTask<DemoNews> GetDemoNewsByIdAsync(this DbSet<DemoNews> dbSet, Guid demoNewsId, CancellationToken cancellationToken = default)
        {
            return dbSet
                    .FindAsync(new object[] { demoNewsId }, cancellationToken);
        }
    }
}
