using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Entities;

namespace CriThink.Server.Core.Repositories
{
    public interface IDebunkingNewsPublisherRepository
    {
        Task<DebunkingNewsPublisher> GetPublisherByNameAsync(
            string publisherName,
            CancellationToken cancellationToken = default);

        Task<DebunkingNewsPublisher> GetPublisherByIdAsync(
            Guid publisherId,
            CancellationToken cancellationToken = default);
    }
}
