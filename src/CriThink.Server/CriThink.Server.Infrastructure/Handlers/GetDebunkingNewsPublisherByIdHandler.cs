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
    internal class GetDebunkingNewsPublisherByIdHandler : IRequestHandler<GetDebunkingNewsPublisherByIdQuery, DebunkingNewsPublisher>
    {
        private readonly CriThinkDbContext _dbContext;
        private readonly ILogger<GetDebunkingNewsPublisherByIdHandler> _logger;

        public GetDebunkingNewsPublisherByIdHandler(CriThinkDbContext dbContext, ILogger<GetDebunkingNewsPublisherByIdHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        public async Task<DebunkingNewsPublisher> Handle(GetDebunkingNewsPublisherByIdQuery request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                var publisher = await _dbContext.DebunkingNewsPublishers
                    .GetPublisherByIdAsync(request.Id, cancellationToken)
                    .ConfigureAwait(false);

                return publisher ?? throw new ResourceNotFoundException($"Can't find a publisher with id '{request.Id}'");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error getting debunking news publisher by id", request.Id);
                throw;
            }
        }
    }
}