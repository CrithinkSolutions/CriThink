using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.UnknownNewsSource;
using CriThink.Server.Core.Entities;
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

        internal static Task<UnknownNewsSourceResponse> GetUnknownNewsSourceByIdAsync(
            this DbSet<UnknownNewsSource> dbSet,
            Guid unknownNewsSourceId,
            Expression<Func<UnknownNewsSource, UnknownNewsSourceResponse>> projection,
            CancellationToken cancellationToken)
        {
            return dbSet.Where(us => us.Id == unknownNewsSourceId)
                        .Select(projection)
                        .SingleOrDefaultAsync(cancellationToken);
        }
    }
}
