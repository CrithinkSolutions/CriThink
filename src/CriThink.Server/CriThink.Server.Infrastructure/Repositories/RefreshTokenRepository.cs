using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Repositories;
using CriThink.Server.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CriThink.Server.Infrastructure.Repositories
{
    internal class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly CriThinkDbContext _dbContext;

        public RefreshTokenRepository(
            CriThinkDbContext dbContext)
        {
            _dbContext = dbContext ??
                throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task DeleteExpiredTokensAsync(
            CancellationToken cancellationToken = default)
        {
            const string sqlCommand = "DELETE FROM refresh_tokens\n" +
                                      "WHERE expires_at < now() AT TIME ZONE 'UTC'";

            await _dbContext.Database.ExecuteSqlRawAsync(sqlCommand, cancellationToken);

            _dbContext.RefreshTokens
                .FromSqlRaw(sqlCommand);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
