using System;
using System.Threading.Tasks;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Repositories;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.Queries
{
    internal class DebunkingNewsPublisherQueries : IDebunkingNewsPublisherQueries
    {
        private readonly IDebunkingNewsPublisherRepository _debunkingNewsPublisherRepository;
        private readonly ILogger<DebunkingNewsPublisherQueries> _logger;

        public DebunkingNewsPublisherQueries(
            IDebunkingNewsPublisherRepository debunkingNewsPublisherRepository,
            ILogger<DebunkingNewsPublisherQueries> logger)
        {
            _debunkingNewsPublisherRepository = debunkingNewsPublisherRepository ??
                throw new ArgumentNullException(nameof(debunkingNewsPublisherRepository));

            _logger = logger;
        }

        public async Task<DebunkingNewsPublisher> GetDebunkingNewsPublisherByNameAsync(string publisherName)
        {
            _logger?.LogInformation(nameof(GetDebunkingNewsPublisherByNameAsync));

            var publisher = await _debunkingNewsPublisherRepository.GetPublisherByNameAsync(publisherName);

            _logger?.LogInformation($"{nameof(GetDebunkingNewsPublisherByNameAsync)}: done");

            return publisher;
        }
    }
}
