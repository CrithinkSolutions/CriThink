using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Queries;
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
        /// <param name="languageFilters">Language filters</param>
        /// <param name="projection">Projection applied to Select query</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Awaitable task</returns>
        internal static Task<List<GetAllDebunkingNewsQueryResponse>> GetAllDebunkingNewsAsync(
            this DbSet<DebunkingNews> dbSet,
            int pageSize,
            int pageIndex,
            GetAllDebunkingNewsLanguageFilters languageFilters,
            Expression<Func<DebunkingNews, GetAllDebunkingNewsQueryResponse>> projection,
            CancellationToken cancellationToken = default)
        {
            var query = dbSet
                .OrderByDescending(dn => dn.PublishingDate)
                .AsQueryable();

            IList<string> languageCodes = null;

            if (languageFilters.HasFlag(GetAllDebunkingNewsLanguageFilters.English))
            {
                languageCodes ??= new List<string>();
                languageCodes.Add(EntityConstants.LanguageCodeEn);
            }

            if (languageFilters.HasFlag(GetAllDebunkingNewsLanguageFilters.Italian))
            {
                languageCodes ??= new List<string>();
                languageCodes.Add(EntityConstants.LanguageCodeIt);
            }

            if (languageCodes != null && languageCodes.Any())
            {
                query = query.Where(dn => languageCodes.Contains(dn.Publisher.Language.Code));
            }

            return query.Skip(pageSize * pageIndex)
                .Take(pageSize + 1)
                .Select(projection)
                .ToListAsync(cancellationToken);
        }

        internal static Task<DebunkingNews> GetDebunkingNewsAsync(
            this DbSet<DebunkingNews> dbSet,
            Guid id,
            CancellationToken cancellationToken = default)
        {
            // TODO: improve query getting only the needed fields

            return dbSet
                .Include(dn => dn.Publisher)
                .Include(dn => dn.Publisher.Language)
                .Include(dn => dn.Publisher.Country)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);
        }
    }
}
