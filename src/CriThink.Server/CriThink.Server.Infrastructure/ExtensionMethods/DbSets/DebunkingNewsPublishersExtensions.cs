using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CriThink.Server.Infrastructure.ExtensionMethods.DbSets
{
    public static class DebunkingNewsPublishersExtensions
    {
        /// <summary>
        /// Returns the publisher by name
        /// </summary>
        /// <param name="dbSet">This <see cref="DbSet{TEntity}"/></param>
        /// <param name="name">Publisher name to search</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Awaitable task</returns>
        public static Task<DebunkingNewsPublisher> GetPublisherByNameAsync(
            this DbSet<DebunkingNewsPublisher> dbSet,
            string name,
            CancellationToken cancellationToken = default)
        {
            return dbSet
                .Include(dnp => dnp.Language)
                .Include(dnp => dnp.Country)
                .SingleOrDefaultAsync(p => p.Name == name, cancellationToken);
        }

        /// <summary>
        /// Returns the publisher by id
        /// </summary>
        /// <param name="dbSet">This <see cref="DbSet{TEntity}"/></param>
        /// <param name="publisherId">Publisher id to search</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Awaitable task</returns>
        public static ValueTask<DebunkingNewsPublisher> GetPublisherByIdAsync(
            this DbSet<DebunkingNewsPublisher> dbSet,
            Guid publisherId,
            CancellationToken cancellationToken = default)
        {
            if (dbSet is null)
                throw new ArgumentNullException(nameof(dbSet));

            return dbSet
                .FindAsync(new object[] { publisherId }, cancellationToken);
        }
    }
}
