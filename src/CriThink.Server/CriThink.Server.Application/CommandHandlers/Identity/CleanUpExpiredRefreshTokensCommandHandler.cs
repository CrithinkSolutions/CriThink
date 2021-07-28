using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Application.Commands;
using CriThink.Server.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.CommandHandlers
{
    internal class CleanUpExpiredRefreshTokensCommandHandler : IRequestHandler<CleanUpExpiredRefreshTokensCommand>
    {
        private readonly CriThinkDbContext _dbContext;
        private readonly ILogger<CleanUpExpiredRefreshTokensCommandHandler> _logger;

        public CleanUpExpiredRefreshTokensCommandHandler(
            CriThinkDbContext dbContext,
            ILogger<CleanUpExpiredRefreshTokensCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Unit> Handle(CleanUpExpiredRefreshTokensCommand request, CancellationToken cancellationToken)
        {
            _logger?.LogInformation("CleanUpExpiredRefreshTokens");

            const string sqlCommand = "DELETE FROM refresh_tokens\n" +
                                      "WHERE expires_at < now() AT TIME ZONE 'UTC'";

            await _dbContext.Database.ExecuteSqlRawAsync(sqlCommand, cancellationToken);

            _dbContext.RefreshTokens
                .FromSqlRaw(sqlCommand);

            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger?.LogInformation("CleanUpExpiredRefreshTokens: done");

            return Unit.Value;
        }
    }
}
