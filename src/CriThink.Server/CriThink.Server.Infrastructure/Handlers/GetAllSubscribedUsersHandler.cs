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
    public class GetAllSubscribedUsersHandler : IRequestHandler<GetAllSubscribedUsersQuery, IList<GetAllSubscribedUsersResponse>>
    {
        private readonly CriThinkDbContext _dbContext;
        private readonly ILogger<GetAllSubscribedUsersHandler> _logger;

        public GetAllSubscribedUsersHandler(CriThinkDbContext dbContext, ILogger<GetAllSubscribedUsersHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        public async Task<IList<GetAllSubscribedUsersResponse>> Handle(GetAllSubscribedUsersQuery request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                var users = await _dbContext.UnknownNewsSourceNotificationRequests
                                                .GetAllSubscribedUsersForUnknownNewsSourceId(
                                                    request.UnknownNewsSourceId,
                                                    request.PageSize,
                                                    request.PageIndex,
                                                    UnknownSourceNotificationRequestProjection.GetAll, cancellationToken)
                                                .ConfigureAwait(false);

                return users;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
