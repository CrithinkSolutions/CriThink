﻿using System;
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
        /// Get news source answers by user id and news link
        /// </summary>
        /// <param name="dbSet">This <see cref="DbSet{TEntity}"/></param>
        /// <param name="userId">User id</param>
        /// <param name="newsLink">News link</param>
        /// <param name="cancellationToken">(Optional) Cancellation token</param>
        /// <returns></returns>
        internal static Task<List<ArticleAnswer>> GetNewsSourceAnswersByUserIdAndNewssLinkAsync(
            this DbSet<ArticleAnswer> dbSet,
            Guid userId,
            string newsLink,
            CancellationToken cancellationToken = default)
        {
            return dbSet
                .Where(aa => aa.UserId == userId && aa.NewsLink == newsLink)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Get news source answers by news link
        /// </summary>
        /// <param name="dbSet">This <see cref="DbSet{TEntity}"/></param>
        /// <param name="newsLink">News link</param>
        /// <param name="cancellationToken">(Optional) Cancellation token</param>
        /// <returns></returns>
        internal static Task<List<ArticleAnswer>> GetNewsSourceAnswersByNewsLinkAsync(
            this DbSet<ArticleAnswer> dbSet,
            string newsLink,
            CancellationToken cancellationToken = default)
        {
            return dbSet
                .Where(aa => aa.NewsLink == newsLink)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}
