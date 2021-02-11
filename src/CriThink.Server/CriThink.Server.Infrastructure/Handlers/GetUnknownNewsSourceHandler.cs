using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.UnknownNewsSource;
using CriThink.Server.Core.Queries;
using CriThink.Server.Infrastructure.Data;
using CriThink.Server.Infrastructure.ExtensionMethods.DbSets;
using CriThink.Server.Infrastructure.Projections;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Infrastructure.Handlers
{
    public class GetUnknownNewsSourceHandler : IRequestHandler<GetUnknownNewsSourceQuery, UnknownNewsSourceResponse>
    {
        private readonly CriThinkDbContext _dbContext;
        private readonly ILogger<GetUnknownNewsSourceHandler> _logger;

        public GetUnknownNewsSourceHandler(CriThinkDbContext dbContext, ILogger<GetUnknownNewsSourceHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        public async Task<UnknownNewsSourceResponse> Handle(GetUnknownNewsSourceQuery request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                var response = await _dbContext.UnknownNewsSources
                                               .GetUnknownNewsSourceByIdAsync(request.UnknownNewsSourceId,
                                                                              UnknownNewsSourceProjection.GetUnknownNewsSource,
                                                                              cancellationToken)
                                               .ConfigureAwait(false);

                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
