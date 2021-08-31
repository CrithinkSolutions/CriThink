using System;
using System.Threading.Tasks;
using CriThink.Server.Core.Entities;

namespace CriThink.Server.Core.DomainServices
{
    public interface IDebunkingNewsPublisherService
    {
        Task<DebunkingNewsPublisher> GetDebunkingNewsPublisherByNameAsync(string publisherName);

        Task<DebunkingNewsPublisher> GetDebunkingNewsPublisherByIdAsync(Guid id);
    }
}
