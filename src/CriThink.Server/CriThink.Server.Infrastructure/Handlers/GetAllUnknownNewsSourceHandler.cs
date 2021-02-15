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
    internal class GetAllUnknownNewsSourceHandler : IRequestHandler<GetAllUnknownSourceQuery, List<GetAllUnknownSources>>
    {
        private readonly CriThinkDbContext _dbContext;
        private readonly ILogger<GetAllUnknownNewsSourceHandler> _logger;

        public GetAllUnknownNewsSourceHandler(CriThinkDbContext dbContext, ILogger<GetAllUnknownNewsSourceHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        public async Task<List<GetAllUnknownSources>> Handle(GetAllUnknownSourceQuery request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                var unknownNewsSources = await _dbContext.UnknownNewsSources
                    .GetAllUnknownSourceAsync(request.Size, request.Index, UnknownNewsSourceProjection.GetAll, cancellationToken)
                    .ConfigureAwait(false);

                return unknownNewsSources;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error getting all unknown news sources");
                throw;
            }
        }
    }
}