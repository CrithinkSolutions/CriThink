using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Application.Commands;
using CriThink.Server.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.CommandHandlers
{
    internal class CleanUpExpiredRefreshTokensCommandHandler : IRequestHandler<CleanUpExpiredRefreshTokensCommand>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ILogger<CleanUpExpiredRefreshTokensCommandHandler> _logger;

        public CleanUpExpiredRefreshTokensCommandHandler(
            IRefreshTokenRepository refreshTokenRepository,
            ILogger<CleanUpExpiredRefreshTokensCommandHandler> logger)
        {
            _refreshTokenRepository = refreshTokenRepository ??
                throw new ArgumentNullException(nameof(refreshTokenRepository));

            _logger = logger;
        }

        public async Task<Unit> Handle(CleanUpExpiredRefreshTokensCommand request, CancellationToken cancellationToken)
        {
            _logger?.LogInformation("CleanUpExpiredRefreshTokens");

            await _refreshTokenRepository.DeleteExpiredTokensAsync();

            _logger?.LogInformation("CleanUpExpiredRefreshTokens: done");

            return Unit.Value;
        }
    }
}
