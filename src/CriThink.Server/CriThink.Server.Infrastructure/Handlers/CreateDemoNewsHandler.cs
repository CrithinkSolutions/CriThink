using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Entities;
using CriThink.Server.Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Infrastructure.Handlers
{
    // ReSharper disable once UnusedMember.Global
    internal class CreateDemoNewsHandler : IRequestHandler<DemoNews>
    {
        private readonly CriThinkDbContext _dbContext;
        private readonly ILogger<CreateDemoNewsHandler> _logger;

        public CreateDemoNewsHandler(CriThinkDbContext dbContext, ILogger<CreateDemoNewsHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        public async Task<Unit> Handle(DemoNews request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                await _dbContext.DemoNews.AddAsync(request, cancellationToken).ConfigureAwait(false);
                await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error adding a demo news", request);
                throw;
            }

            return Unit.Value;
        }
    }
}