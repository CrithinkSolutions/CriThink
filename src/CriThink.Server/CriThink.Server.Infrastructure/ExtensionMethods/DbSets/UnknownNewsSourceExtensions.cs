using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.QueryResults;
using Microsoft.EntityFrameworkCore;

namespace CriThink.Server.Infrastructure.ExtensionMethods.DbSets
{
    internal static class UnknownNewsSourceExtensions
    {
        /// <summary>
        /// Get an unknown source id by uri
        /// </summary>
        /// <param name="dbSet">This <see cref="DbSet{TEntity}"/></param>
        /// <param name="uri">Unknown source uri</param>
        /// <param name="projection">Projection applied to Select query</param>
        /// <param name="cancellationToken">(Optional) Cancellation token</param>
        /// <returns></returns>
        internal static Task<Guid> GetUnknownSourceIdByUriAsync(
            this DbSet<UnknownNewsSource> dbSet,
            string uri,
            Expression<Func<UnknownNewsSource, Guid>> projection,
            CancellationToken cancellationToken = default)
        {
            return dbSet.Where(us => us.Uri == uri)
                        .Select(projection)
                        .SingleOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// Get an unknown source by id
        /// </summary>
        /// <param name="dbSet">This <see cref="DbSet{TEntity}"/></param>
        /// <param name="unknownNewsSourceId">Unknown source id</param>
        /// <param name="projection">Projection applied to Select query</param>
        /// <param name="cancellationToken">(Optional) Cancellation token</param>
        /// <returns></returns>
        internal static Task<UnknownNewsSource> GetUnknownNewsSourceByIdAsync(
            this DbSet<UnknownNewsSource> dbSet,
            Guid unknownNewsSourceId,
            Expression<Func<UnknownNewsSource, UnknownNewsSource>> projection,
            CancellationToken cancellationToken = default)
        {
            return dbSet.Where(us => us.Id == unknownNewsSourceId)
                        .Select(projection)
                        .SingleOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// Get all unknown sources
        /// </summary>
        /// <param name="dbSet">This <see cref="DbSet{TEntity}"/></param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="projection">Projection applied to Select query</param>
        /// <param name="cancellationToken">(Optional) Cancellation token</param>
        /// <returns></returns>
        internal static Task<List<GetAllUnknownSourcesQueryResult>> GetAllUnknownSourceAsync(
            this DbSet<UnknownNewsSource> dbSet,
            int pageSize,
            int pageIndex,
            Expression<Func<UnknownNewsSource, GetAllUnknownSourcesQueryResult>> projection,
            CancellationToken cancellationToken = default)
        {
            return dbSet
                .OrderByDescending(r => r.FirstRequestedAt)
                .Skip(pageIndex * pageSize)
                .Take(pageSize + 1)
                .Select(projection)
                .ToListAsync(cancellationToken);
        }
    }
}
