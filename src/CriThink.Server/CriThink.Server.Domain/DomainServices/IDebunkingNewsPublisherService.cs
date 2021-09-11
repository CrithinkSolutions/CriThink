using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Domain.Entities;

namespace CriThink.Server.Domain.DomainServices
{
    public interface IDebunkingNewsPublisherService
    {
        Task<DebunkingNewsPublisher> GetDebunkingNewsPublisherByNameAsync(
            string publisherName,
            CancellationToken cancellationToken = default);

        Task<DebunkingNewsPublisher> GetDebunkingNewsPublisherByIdAsync(
            Guid id,
            CancellationToken cancellationToken);
    }
}
