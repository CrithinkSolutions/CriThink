using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CriThink.Server.Infrastructure.ExtensionMethods.DbSets
{
    internal static class UnknownNewsSourceExtensions
    {
        internal static Task<Guid> GetUnknownSourceByUri(this DbSet<UnknownNewsSource> dbSet, string uri, Expression<Func<UnknownNewsSource, Guid>> projection, CancellationToken cancellationToken)
        {
            return dbSet.Where(us => us.Uri == uri)
                        .Select(projection)
                        .SingleOrDefaultAsync(cancellationToken);
        }
    }
}
