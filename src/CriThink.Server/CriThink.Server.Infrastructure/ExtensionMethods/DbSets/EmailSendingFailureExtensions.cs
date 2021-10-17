using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CriThink.Server.Infrastructure.ExtensionMethods.DbSets
{
    internal static class EmailSendingFailureExtensions
    {
        /// <summary>
        /// Returns email sending failures
        /// </summary>
        /// <param name="dbSet">This <see cref="DbSet{TEntity}"/></param>
        /// <param name="cancellationToken">(Optional) Cancellation token</param>
        /// <returns></returns>
        internal static Task<List<EmailSendingFailure>> GetAllEmailSendingFailuresAsync(
            this DbSet<EmailSendingFailure> dbSet,
            CancellationToken cancellationToken = default)
        {
            return dbSet
                .ToListAsync(cancellationToken);
        }
    }
}
