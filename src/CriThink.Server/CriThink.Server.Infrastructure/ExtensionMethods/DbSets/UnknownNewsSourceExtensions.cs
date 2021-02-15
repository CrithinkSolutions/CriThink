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
    internal static class UnknownNewsSourceExtensions
    {
        internal static Task<Guid> GetUnknownSourceIdByUriAsync(
            this DbSet<UnknownNewsSource> dbSet,
            string uri,
            Expression<Func<UnknownNewsSource, Guid>> projection,
            CancellationToken cancellationToken)
        {
            return dbSet.Where(us => us.Uri == uri)
                        .Select(projection)
                        .SingleOrDefaultAsync(cancellationToken);
        }

        internal static Task<UnknownNewsSource> GetUnknownNewsSourceByIdAsync(
            this DbSet<UnknownNewsSource> dbSet,
            Guid unknownNewsSourceId,
            Expression<Func<UnknownNewsSource, UnknownNewsSource>> projection,
            CancellationToken cancellationToken)
        {
            return dbSet.Where(us => us.Id == unknownNewsSourceId)
                        .Select(projection)
                        .SingleOrDefaultAsync(cancellationToken);
        }

        internal static Task<List<GetAllUnknownSources>> GetAllUnknownSourceAsync(
            this DbSet<UnknownNewsSource> dbSet,
            int pageSize,
            int pageIndex,
            Expression<Func<UnknownNewsSource, GetAllUnknownSources>> projection,
            CancellationToken cancellationToken)
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
