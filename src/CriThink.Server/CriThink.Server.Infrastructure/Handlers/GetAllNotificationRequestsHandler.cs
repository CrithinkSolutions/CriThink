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
    internal class GetAllNotificationRequestsHandler : IRequestHandler<GetAllNotificationRequestsQuery, List<GetAllSubscribedUsersWithSourceResponse>>
    {
        private readonly CriThinkDbContext _dbContext;
        private readonly ILogger<GetAllNotificationRequestsHandler> _logger;

        public GetAllNotificationRequestsHandler(CriThinkDbContext dbContext, ILogger<GetAllNotificationRequestsHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        public async Task<List<GetAllSubscribedUsersWithSourceResponse>> Handle(GetAllNotificationRequestsQuery request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                var pendingNotifications = await _dbContext.UnknownNewsSourceNotificationRequests
                    .GetAllNotificationRequestsAsync(request.Size, request.Index, UnknownSourceNotificationRequestProjection.GetAllWithSources, cancellationToken)
                    .ConfigureAwait(false);

                return pendingNotifications;

            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error getting all pending notification requests");
                throw;
            }
        }
    }
}
