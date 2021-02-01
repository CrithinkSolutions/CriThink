using System.Linq;
using CriThink.Server.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CriThink.Server.Infrastructure.ExtensionMethods.DbSets
{
    internal static class DbSetExtensions
    {
        internal static IQueryable<TEntity> ApplyPagination<TEntity>(this DbSet<TEntity> dbSet, int pageSize, int pageIndex) where TEntity : class, ICriThinkIdentity
        {
            return dbSet
                .Skip(pageSize * pageIndex)
                .Take(pageSize + 1);
        }
    }
}
