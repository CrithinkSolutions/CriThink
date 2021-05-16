using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Commands;
using CriThink.Server.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Infrastructure.Handlers
{
    internal class DeleteRefreshTokenHandler : IRequestHandler<DeleteRefreshTokenCommand>
    {
        private readonly CriThinkDbContext _dbContext;
        private readonly ILogger<DeleteRefreshTokenHandler> _logger;

        public DeleteRefreshTokenHandler(CriThinkDbContext dbContext, ILogger<DeleteRefreshTokenHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                const string sqlCommand = "DELETE FROM refresh_tokens\n" +
                                          "WHERE expires_at < now() AT TIME ZONE 'UTC'";

                await _dbContext.Database.ExecuteSqlRawAsync(sqlCommand, cancellationToken)
                    .ConfigureAwait(false);

                _dbContext.RefreshTokens
                    .FromSqlRaw(sqlCommand);

                await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error deleting refresh tokens");
                throw;
            }

            return Unit.Value;
        }
    }
}
