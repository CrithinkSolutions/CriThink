using System;
using System.Threading.Tasks;
using CriThink.Server.Core.DomainServices;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Repositories;

namespace CriThink.Server.Application.DomainServices
{
    internal class DebunkingNewsPublisherService : IDebunkingNewsPublisherService
    {
        private readonly IDebunkingNewsPublisherRepository _debunkingNewsPublisherRepository;

        public DebunkingNewsPublisherService(
            IDebunkingNewsPublisherRepository debunkingNewsPublisherRepository)
        {
            _debunkingNewsPublisherRepository = debunkingNewsPublisherRepository ??
                throw new ArgumentNullException(nameof(debunkingNewsPublisherRepository));
        }

        public async Task<DebunkingNewsPublisher> GetDebunkingNewsPublisherByNameAsync(string publisherName)
        {
            var debunkingNewsPublisher = await _debunkingNewsPublisherRepository.GetPublisherByNameAsync(publisherName);
            return debunkingNewsPublisher;
        }

        public async Task<DebunkingNewsPublisher> GetDebunkingNewsPublisherByIdAsync(Guid id)
        {
            var debunkingNewsPublisher = await _debunkingNewsPublisherRepository.GetPublisherByIdAsync(id);
            return debunkingNewsPublisher;
        }
    }
}
