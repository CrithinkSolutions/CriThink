using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Domain.Entities;

namespace CriThink.Server.Application.Queries
{
    public interface IDebunkingNewsPublisherQueries
    {
        Task<DebunkingNewsPublisher> GetDebunkingNewsPublisherByNameAsync(
            string publisherName,
            CancellationToken cancellationToken = default);

        Task<DebunkingNewsPublisher> GetDebunkingNewsPublisherByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<IList<DebunkingNewsPublisher>> GetAllDebunkingNewsPublishersAsync(
            CancellationToken cancellationToken = default);
    }
}
