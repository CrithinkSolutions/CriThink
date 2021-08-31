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
    internal static class DebunkingNewsExtensions
    {
        /// <summary>
        /// Returns paginated debunking news
        /// </summary>
        /// <param name="dbSet">This <see cref="DbSet{TEntity}"/></param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="projection">Projection applied to Select query</param>
        /// <param name="languageFilter">(Optional) Language filters</param>
        /// <param name="cancellationToken">(Optional) Cancellation token</param>
        /// <returns>Awaitable task</returns>
        internal static Task<List<GetAllDebunkingNewsQueryResult>> GetAllDebunkingNewsAsync(
            this DbSet<DebunkingNews> dbSet,
            int pageSize,
            int pageIndex,
            Expression<Func<DebunkingNews, GetAllDebunkingNewsQueryResult>> projection,
            string languageFilter = null,
            CancellationToken cancellationToken = default)
        {
            var query = dbSet
                .OrderByDescending(dn => dn.PublishingDate)
                .AsQueryable();

            IList<string> languageCodes = languageFilter
                .Split(',')
                .ToList();

            if (languageCodes?.Any() == true)
            {
                query = query.Where(dn => languageCodes.Contains(dn.Publisher.Language.Code));
            }

            return query.Skip(pageSize * pageIndex)
                .Take(pageSize + 1)
                .Select(projection)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Get debunking news by id
        /// </summary>
        /// <param name="dbSet">This <see cref="DbSet{TEntity}"/></param>
        /// <param name="id">Debunking news id</param>
        /// <param name="cancellationToken">(Optional) Cancellation token</param>
        /// <returns></returns>
        internal static Task<DebunkingNews> GetDebunkingNewsAsync(
            this DbSet<DebunkingNews> dbSet,
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return dbSet
                .Include(dn => dn.Publisher)
                .Include(dn => dn.Publisher.Language)
                .Include(dn => dn.Publisher.Country)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);
        }
    }
}
