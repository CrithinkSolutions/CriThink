using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Queries;
using CriThink.Server.Core.Responses;
using CriThink.Server.Infrastructure.Data;
using CriThink.Server.Infrastructure.ExtensionMethods.DbSets;
using CriThink.Server.Infrastructure.Projections;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Infrastructure.Handlers
{
    // ReSharper disable once UnusedMember.Global
    internal class GetAllTriggerLogsHandler : IRequestHandler<GetAllTriggerLogsQuery, IList<GetAllTriggerLogQueryResponse>>
    {
        private readonly CriThinkDbContext _dbContext;
        private readonly ILogger<GetAllTriggerLogsHandler> _logger;

        public GetAllTriggerLogsHandler(CriThinkDbContext dbContext, ILogger<GetAllTriggerLogsHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        public async Task<IList<GetAllTriggerLogQueryResponse>> Handle(GetAllTriggerLogsQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                var allDebunkingNews = await _dbContext.DebunkingNewsTriggerLogs
                    .GetAllDebunkingNewsTriggerLogsAsync(request.Size, request.Index, DebunkingNewsTriggerLogsProjection.GetAll, cancellationToken)
                    .ConfigureAwait(false);

                return allDebunkingNews;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error getting all debunking news trigger logs", request);
                throw;
            }
        }
    }
}
