using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Exceptions;
using CriThink.Server.Core.Queries;
using CriThink.Server.Infrastructure.Data;
using CriThink.Server.Infrastructure.ExtensionMethods.DbSets;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Infrastructure.Handlers
{
    internal class GetDebunkingNewsPublisherByNameHandler : IRequestHandler<GetDebunkingNewsPublisherByNameQuery, DebunkingNewsPublisher>
    {
        private readonly CriThinkDbContext _dbContext;
        private readonly ILogger<GetDebunkingNewsPublisherByNameHandler> _logger;

        public GetDebunkingNewsPublisherByNameHandler(CriThinkDbContext dbContext, ILogger<GetDebunkingNewsPublisherByNameHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        public async Task<DebunkingNewsPublisher> Handle(GetDebunkingNewsPublisherByNameQuery request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                var publisher = await _dbContext.DebunkingNewsPublishers
                    .GetPublisherByNameAsync(request.Name, cancellationToken)
                    .ConfigureAwait(false);

                return publisher ?? throw new ResourceNotFoundException($"Can't find a publisher with name '{request.Name}'");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error getting debunking news publisher by name", request.Name);
                throw;
            }
        }
    }
}
