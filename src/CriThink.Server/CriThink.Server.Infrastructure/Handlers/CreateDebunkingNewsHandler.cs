using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Commands;
using CriThink.Server.Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Infrastructure.Handlers
{
    // ReSharper disable once UnusedMember.Global
    internal class CreateDebunkingNewsHandler : IRequestHandler<CreateDebunkingNewsCommand>
    {
        private readonly CriThinkDbContext _dbContext;
        private readonly ILogger<CreateDebunkingNewsCommand> _logger;

        public CreateDebunkingNewsHandler(CriThinkDbContext dbContext, ILogger<CreateDebunkingNewsCommand> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        public async Task<Unit> Handle(CreateDebunkingNewsCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                await _dbContext.DebunkedNews.AddRangeAsync(request.DebunkedNewsCollection, cancellationToken).ConfigureAwait(false);
                await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error adding debunked news collection", request);
                throw;
            }

            return Unit.Value;
        }
    }
}
