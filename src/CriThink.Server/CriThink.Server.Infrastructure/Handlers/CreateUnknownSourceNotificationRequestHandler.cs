using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Commands;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Exceptions;
using CriThink.Server.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Infrastructure.Handlers
{
    public class CreateUnknownSourceNotificationRequestHandler : IRequestHandler<CreateUnknownSourceNotificationRequestCommand>
    {
        private readonly CriThinkDbContext _dbContext;
        private readonly ILogger<CreateUnknownSourceNotificationRequestHandler> _logger;

        public CreateUnknownSourceNotificationRequestHandler(CriThinkDbContext dbContext, ILogger<CreateUnknownSourceNotificationRequestHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        public async Task<Unit> Handle(CreateUnknownSourceNotificationRequestCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                var unknownNewsSource = await GetUnknownNewsSourceByUriAsync(request.Domain, cancellationToken)
                    .ConfigureAwait(false);

                if (unknownNewsSource is null)
                    throw new ResourceNotFoundException($"Can't find an unknown source with url '{request.Domain}'");

                var unknownSourcesNotificationRequest = new UnknownNewsSourceNotificationRequest
                {
                    Email = request.Email,
                    UnknownNewsSource = unknownNewsSource,
                    RequestedAt = DateTime.Now,
                };

                await AddNotificationRequestAsync(unknownSourcesNotificationRequest, cancellationToken)
                    .ConfigureAwait(false);

                return Unit.Value;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, ex.Message);
                throw;
            }
        }

        private Task<UnknownNewsSource> GetUnknownNewsSourceByUriAsync(string url, CancellationToken cancellationToken) =>
            _dbContext.UnknownNewsSources
                .SingleOrDefaultAsync(uns => uns.Uri == url, cancellationToken);

        private async Task AddNotificationRequestAsync(UnknownNewsSourceNotificationRequest entity, CancellationToken cancellationToken)
        {
            await _dbContext.UnknownNewsSourceNotificationRequests.AddAsync(entity, cancellationToken)
                .ConfigureAwait(false);

            await _dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
