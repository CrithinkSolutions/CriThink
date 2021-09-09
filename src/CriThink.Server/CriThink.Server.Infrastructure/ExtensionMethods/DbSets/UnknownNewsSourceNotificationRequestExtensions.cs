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
    internal static class UnknownNewsSourceNotificationRequestExtensions
    {
        /// <summary>
        /// Get the user list subscribed to a specific news source
        /// </summary>
        /// <param name="dbSet">This <see cref="DbSet{TEntity}"/></param>
        /// <param name="unknownNewsSourceId">Unknown source id</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="projection">Projection applied to Select query</param>
        /// <param name="cancellationToken">(Optional) Cancellation token</param>
        /// <returns></returns>
        internal static Task<List<GetAllSubscribedUsersQueryResult>> GetAllSubscribedUsersForUnknownNewsSourceId(
            this DbSet<UnknownNewsSourceNotification> dbSet,
            Guid unknownNewsSourceId,
            int pageSize,
            int pageIndex,
            Expression<Func<UnknownNewsSourceNotification, GetAllSubscribedUsersQueryResult>> projection,
            CancellationToken cancellationToken = default)
        {
            return dbSet.Where(unsnr => unsnr.UnknownNewsSource.Id == unknownNewsSourceId)
                         .OrderBy(unsnr => unsnr.RequestedAt)
                         .Skip(pageIndex * pageSize)
                         .Take(pageSize)
                         .Select(projection)
                         .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Get the notification request for the given user
        /// </summary>
        /// <param name="dbSet">This <see cref="DbSet{TEntity}"/></param>
        /// <param name="email">User email</param>
        /// <param name="link">News link</param>
        /// <param name="cancellationToken">(Optional) Cancellation token</param>
        /// <returns></returns>
        internal static Task<UnknownNewsSourceNotification> GetNotificationRequestByEmailAndLiknAsync(
            this DbSet<UnknownNewsSourceNotification> dbSet,
            string email,
            string link,
            CancellationToken cancellationToken = default)
        {
            return dbSet
                .FirstOrDefaultAsync(n => n.Email == email && n.UnknownNewsSource.Uri == link, cancellationToken);
        }

        /// <summary>
        /// Get all notification requests
        /// </summary>
        /// <param name="dbSet">This <see cref="DbSet{TEntity}"/></param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="projection">Projection applied to Select query</param>
        /// <param name="cancellationToken">(Optional) Cancellation token</param>
        /// <returns></returns>
        internal static Task<List<GetAllSubscribedUsersWithSourceQueryResult>> GetAllNotificationRequestsAsync(
            this DbSet<UnknownNewsSourceNotification> dbSet,
            int pageSize,
            int pageIndex,
            Expression<Func<UnknownNewsSourceNotification, GetAllSubscribedUsersWithSourceQueryResult>> projection,
            CancellationToken cancellationToken = default)
        {
            return dbSet
                .OrderBy(r => r.RequestedAt)
                .Skip(pageIndex * pageSize)
                .Take(pageSize + 1)
                .Select(projection)
                .ToListAsync(cancellationToken);
        }
    }
}
