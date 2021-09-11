using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Application.Queries;
using CriThink.Server.Domain.DomainServices;
using CriThink.Server.Domain.Entities;

namespace CriThink.Server.Application.DomainServices
{
    internal class DebunkingNewsPublisherService : IDebunkingNewsPublisherService
    {
        private readonly IDebunkingNewsPublisherQueries _debunkingNewsPublisherQueries;

        public DebunkingNewsPublisherService(
            IDebunkingNewsPublisherQueries debunkingNewsPublisherQueries)
        {
            _debunkingNewsPublisherQueries = debunkingNewsPublisherQueries ??
                throw new ArgumentNullException(nameof(debunkingNewsPublisherQueries));
        }

        public async Task<DebunkingNewsPublisher> GetDebunkingNewsPublisherByNameAsync(
            string publisherName,
            CancellationToken cancellationToken = default)
        {
            var debunkingNewsPublisher = await _debunkingNewsPublisherQueries.GetDebunkingNewsPublisherByNameAsync(
                publisherName,
                cancellationToken);

            return debunkingNewsPublisher;
        }

        public async Task<DebunkingNewsPublisher> GetDebunkingNewsPublisherByIdAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            var debunkingNewsPublisher = await _debunkingNewsPublisherQueries.GetDebunkingNewsPublisherByIdAsync(
                id, cancellationToken);

            return debunkingNewsPublisher;
        }
    }
}
