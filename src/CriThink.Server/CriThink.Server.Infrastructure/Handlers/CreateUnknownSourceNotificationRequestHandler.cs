using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Commands;
using CriThink.Server.Core.Entities;
using CriThink.Server.Infrastructure.Data;
using MediatR;
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
                var unknownSourcesNotificationRequest = new UnknownNewsSourceNotificationRequest
                {
                    Email = request.Email,
                    UnknownNewsSourceId = request.NewsSourceId,
                    RequestedAt = DateTime.Now,
                };

                await _dbContext.UnknownNewsSourceNotificationRequests.AddAsync(unknownSourcesNotificationRequest, cancellationToken).ConfigureAwait(false);

                await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                return Unit.Value;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
