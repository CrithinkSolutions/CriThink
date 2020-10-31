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
    internal class DeleteDebunkingNewsHandler : IRequestHandler<DeleteDebunkingNewsCommand>
    {
        private readonly CriThinkDbContext _dbContext;
        private readonly ILogger<DeleteDebunkingNewsHandler> _logger;

        public DeleteDebunkingNewsHandler(CriThinkDbContext dbContext, ILogger<DeleteDebunkingNewsHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteDebunkingNewsCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            try
            {

                var debunkingNews = await _dbContext.DebunkingNews.FindAsync(new object[] { request.DebunkingNewsId }, cancellationToken: cancellationToken)
                    .ConfigureAwait(false);
                _dbContext.DebunkingNews.Remove(debunkingNews);

                await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error deleting a debunking news", request);
                throw;
            }

            return Unit.Value;
        }
    }
}
