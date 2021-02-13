﻿using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CriThink.Server.Infrastructure.ExtensionMethods.DbSets
{
    internal static class DebunkingNewsPublishersExtensions
    {
        /// <summary>
        /// Returns the publisher by name
        /// </summary>
        /// <param name="dbSet">This <see cref="DbSet{TEntity}"/></param>
        /// <param name="name">Publisher name to search</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Awaitable task</returns>
        internal static Task<DebunkingNewsPublisher> GetPublisherByNameAsync(
            this DbSet<DebunkingNewsPublisher> dbSet,
            string name,
            CancellationToken cancellationToken = default)
        {
            return dbSet
                .SingleOrDefaultAsync(p => p.Name == name, cancellationToken);
        }

        /// <summary>
        /// Returns the publisher by id
        /// </summary>
        /// <param name="dbSet">This <see cref="DbSet{TEntity}"/></param>
        /// <param name="publisherId">Publisher id to search</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Awaitable task</returns>
        internal static ValueTask<DebunkingNewsPublisher> GetPublisherByIdAsync(
            this DbSet<DebunkingNewsPublisher> dbSet,
            Guid publisherId,
            CancellationToken cancellationToken = default)
        {
            return dbSet
                .FindAsync(new object[] { publisherId }, cancellationToken);
        }
    }
}