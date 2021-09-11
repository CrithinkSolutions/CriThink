using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CriThink.Server.Infrastructure.ExtensionMethods.DbSets
{
    internal static class NewsSourceCategoriesExtensions
    {
        /// <summary>
        /// Get category description of the given authenticity grade
        /// </summary>
        /// <param name="dbSet">This <see cref="DbSet{TEntity}"/></param>
        /// <param name="projection">Projection applied to Select query</param>
        /// <param name="authenticity">Authenticty value to search</param>
        /// <param name="cancellationToken">(Optional) Cancellation token</param>
        /// <returns></returns>
        internal static Task<string> GetCategoryDescriptionByAuthenticityAsync(
            this DbSet<NewsSourceCategory> dbSet,
            Expression<Func<NewsSourceCategory, string>> projection,
            NewsSourceAuthenticity authenticity,
            CancellationToken cancellationToken = default)
        {
            return dbSet
                .Where(q => q.Authenticity == authenticity)
                .Select(projection)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
