using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Commands;
using CriThink.Server.Core.Exceptions;
using CriThink.Server.Infrastructure.Data;
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

        public Task<Guid> Handle(GetUnknownNewsSourceIdCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                var newsSource = _dbContext.UnknownSources.SingleOrDefault(_ => _.Uri == request.Uri);

                if (newsSource is null)
                    throw new ResourceNotFoundException($"Cannot find '{request.Uri}' in unknown news sources.");

                return Task.FromResult(newsSource.Id);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
