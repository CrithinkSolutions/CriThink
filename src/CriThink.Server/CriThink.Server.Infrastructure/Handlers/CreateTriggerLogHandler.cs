using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Commands;
using CriThink.Server.Core.Entities;
using CriThink.Server.Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Infrastructure.Handlers
{
    // ReSharper disable once UnusedMember.Global
    internal class CreateTriggerLogHandler : IRequestHandler<CreateTriggerLogCommand>
    {
        private readonly CriThinkDbContext _dbContext;
        private readonly ILogger<CreateTriggerLogHandler> _logger;

        public CreateTriggerLogHandler(CriThinkDbContext dbContext, ILogger<CreateTriggerLogHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        public async Task<Unit> Handle(CreateTriggerLogCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                var triggerLog = new DebunkingNewsTriggerLog
                {
                    IsSuccessful = request.IsSuccessful,
                    TimeStamp = DateTime.UtcNow.ToString("u", CultureInfo.InvariantCulture),
                    FailReason = request.FailReason
                };

                await _dbContext.DebunkingNewsTriggerLogs.AddAsync(triggerLog, cancellationToken).ConfigureAwait(false);
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
