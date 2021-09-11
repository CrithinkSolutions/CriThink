using System;
using System.Threading;
using System.Threading.Tasks;

namespace CriThink.Server.Domain.Repositories
{
    public interface IStatisticsRepository
    {
        Task<long> GetTotalNumberOfUserAsync(CancellationToken cancellationToken = default);

        Task<long> GetTotalNumberOfSearchesAsync(CancellationToken cancellationToken = default);

        Task<long> GetTotalNumberOfUserSearchesAsync(Guid userId, CancellationToken cancellationToken = default);

        Task<long> GetTotalNumberOfRatesArticlesAsync(CancellationToken cancellationToken = default);
    }
}
