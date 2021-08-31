using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Repositories;
using CriThink.Server.Infrastructure.Data;
using CriThink.Server.Infrastructure.ExtensionMethods.DbSets;
using Microsoft.EntityFrameworkCore;

namespace CriThink.Server.Infrastructure.Repositories
{
    internal class DebunkingNewsPublisherRepository : IDebunkingNewsPublisherRepository
    {
        private readonly CriThinkDbContext _dbContext;

        public DebunkingNewsPublisherRepository(
            CriThinkDbContext dbContext)
        {
            _dbContext = dbContext ??
                throw new ArgumentNullException(nameof(dbContext));
        }

        public IUnitOfWork UnitOfWork => _dbContext;

        public async Task<DebunkingNewsPublisher> GetPublisherByNameAsync(
            string publisherName,
            CancellationToken cancellationToken = default)
        {
            var publisher = await _dbContext.DebunkingNewsPublishers
                .GetPublisherByNameAsync(publisherName, cancellationToken);

            return publisher;
        }

        public async Task<DebunkingNewsPublisher> GetPublisherByIdAsync(
            Guid publisherId,
            CancellationToken cancellationToken = default)
        {
            var publisher = await _dbContext.DebunkingNewsPublishers
                    .GetPublisherByIdAsync(publisherId, cancellationToken);

            return publisher;
        }
    }
}
