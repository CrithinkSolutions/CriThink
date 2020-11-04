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
    internal static class DebunkingNewsExtensions
    {
        /// <summary>
        /// Returns paginated debunking news
        /// </summary>
        /// <param name="dbSet">This <see cref="DbSet{TEntity}"/></param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="projection">Projection applied to Select query</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns></returns>
        internal static Task<List<GetAllDebunkingNewsQueryResponse>> GetAllDebunkingNewsAsync(
            this DbSet<DebunkingNews> dbSet,
            int pageSize,
            int pageIndex,
            Expression<Func<DebunkingNews, GetAllDebunkingNewsQueryResponse>> projection,
            CancellationToken cancellationToken = default)
        {
            return dbSet
                .Skip(pageSize * (pageIndex - 1))
                .Take(pageSize)
                .Select(projection)
                .ToListAsync(cancellationToken);
        }

        internal static ValueTask<DebunkingNews> GetDebunkingNewsAsync(
            this DbSet<DebunkingNews> dbSet,
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return dbSet
                .FindAsync(cancellationToken: cancellationToken, keyValues: new object[] { id });
        }
    }
}
