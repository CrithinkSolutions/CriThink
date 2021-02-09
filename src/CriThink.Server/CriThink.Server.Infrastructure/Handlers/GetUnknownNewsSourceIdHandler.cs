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
    public class GetUnknownNewsSourceIdHandler : IRequestHandler<GetUnknownNewsSourceIdCommand, Guid>
    {
        private readonly CriThinkDbContext _dbContext;
        private readonly ILogger<GetUnknownNewsSourceIdHandler> _logger;

        public GetUnknownNewsSourceIdHandler(CriThinkDbContext dbContext, ILogger<GetUnknownNewsSourceIdHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        public async Task<Guid> Handle(GetUnknownNewsSourceIdCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                var newsSourceId = await _dbContext.UnknownNewsSources
                                                   .GetUnknownSourceByUri(request.Uri, UnknownNewsSourceProjection.GetId, cancellationToken)
                                                   .ConfigureAwait(false);
                return newsSourceId;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
