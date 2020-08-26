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
    public static class DemoNewsExtensions
    {
        /// <summary>
        /// Get all the demo news
        /// </summary>
        /// <param name="dbSet">This <see cref="DbSet{TEntity}"/></param>
        /// <param name="projection">Projection applied to Select query</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns></returns>
        public static Task<List<GetAllDemoNewsQueryResponse>> GetAllDemoNewsAsync(this DbSet<DemoNews> dbSet, Expression<Func<DemoNews, GetAllDemoNewsQueryResponse>> projection, CancellationToken cancellationToken = default)
        {
            return dbSet
                .Select(projection)
                .ToListAsync(cancellationToken);
        }
    }
}
