using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CriThink.Server.Infrastructure.ExtensionMethods.DbSets
{
    internal static class NewsSourceAnswersExtensions
    {
        /// <summary>
        /// Get news source answers by user id
        /// </summary>
        /// <param name="dbSet">This <see cref="DbSet{TEntity}"/></param>
        /// <param name="userId">User id</param>
        /// <param name="newsLink">News link</param>
        /// <param name="cancellationToken">(Optional) Cancellation token</param>
        /// <returns></returns>
        internal static Task<List<ArticleAnswer>> GetNewsSourceAnswersByUserIdAsync(
            this DbSet<ArticleAnswer> dbSet,
            Guid userId,
            string newsLink,
            CancellationToken cancellationToken = default)
        {
            return dbSet
                .Where(aa => aa.Id == userId && aa.NewsLink == newsLink)
                .ToListAsync(cancellationToken);
        }
    }
}
