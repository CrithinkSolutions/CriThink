using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Domain.Entities;
using CriThink.Server.Infrastructure.Data;
using CriThink.Server.Infrastructure.ExtensionMethods.DbSets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.Queries
{
    internal class DebunkingNewsPublisherQueries : IDebunkingNewsPublisherQueries
    {
        private readonly CriThinkDbContext _dbContext;
        private readonly ILogger<DebunkingNewsPublisherQueries> _logger;

        public DebunkingNewsPublisherQueries(
            CriThinkDbContext dbContext,
            ILogger<DebunkingNewsPublisherQueries> logger)
        {
            _dbContext = dbContext ??
                throw new ArgumentNullException(nameof(dbContext));

            _logger = logger;
        }

        public async Task<DebunkingNewsPublisher> GetDebunkingNewsPublisherByNameAsync(
            string publisherName,
            CancellationToken cancellationToken = default)
        {
            _logger?.LogInformation(nameof(GetDebunkingNewsPublisherByNameAsync));

            var publisher = await _dbContext.DebunkingNewsPublishers
                .GetPublisherByNameAsync(publisherName, cancellationToken);

            _logger?.LogInformation($"{nameof(GetDebunkingNewsPublisherByNameAsync)}: done");

            return publisher;
        }

        public async Task<DebunkingNewsPublisher> GetDebunkingNewsPublisherByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var debunkingNewsPublisher = await _dbContext.DebunkingNewsPublishers
                .GetPublisherByIdAsync(id, cancellationToken);

            return debunkingNewsPublisher;
        }

        public async Task<IList<DebunkingNewsPublisher>> GetAllDebunkingNewsPublishersAsync(
            CancellationToken cancellationToken = default)
        {
            var debunkingNewsPublishers = await _dbContext.DebunkingNewsPublishers
                .ToListAsync(cancellationToken);

            return debunkingNewsPublishers;
        }
    }
}
