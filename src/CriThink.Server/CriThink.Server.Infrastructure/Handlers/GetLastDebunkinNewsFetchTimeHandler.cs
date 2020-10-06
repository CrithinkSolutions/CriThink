using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Queries;
using CriThink.Server.Infrastructure.Data;
using CriThink.Server.Infrastructure.ExtensionMethods.DbSets;
using CriThink.Server.Infrastructure.Projections;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Infrastructure.Handlers
{
    // ReSharper disable once UnusedMember.Global
    internal class GetLastDebunkinNewsFetchTimeHandler : IRequestHandler<GetLastDebunkinNewsFetchTimeQuery, DateTime>
    {
        private readonly CriThinkDbContext _dbContext;
        private readonly ILogger<GetLastDebunkinNewsFetchTimeHandler> _logger;

        public GetLastDebunkinNewsFetchTimeHandler(CriThinkDbContext dbContext, ILogger<GetLastDebunkinNewsFetchTimeHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        public async Task<DateTime> Handle(GetLastDebunkinNewsFetchTimeQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                var latestEntry = await _dbContext.DebunkingNewsTriggerLogs
                    .GetMostRecentSuccessfullDateAsync(DebunkingNewsTriggerLogsProjection.GetTimeStamp, cancellationToken)
                    .ConfigureAwait(false);

                return latestEntry;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error getting latest fetch time", request);
                throw;
            }
        }
    }
}
