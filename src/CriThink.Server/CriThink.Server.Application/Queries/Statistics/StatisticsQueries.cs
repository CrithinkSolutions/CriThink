using System;
using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.Statistics;
using CriThink.Server.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.Queries
{
    internal class StatisticsQueries : IStatisticsQueries
    {
        private readonly IStatisticsRepository _statisticsRepository;
        private readonly ILogger<StatisticsQueries> _logger;

        public StatisticsQueries(
            IStatisticsRepository statisticsRepository,
            ILogger<StatisticsQueries> logger)
        {
            _statisticsRepository = statisticsRepository ??
                throw new ArgumentNullException(nameof(statisticsRepository));

            _logger = logger;
        }

        public async Task<PlatformDataUsageResponse> GetPlatformUsageDataAsync()
        {
            _logger?.LogInformation(nameof(GetPlatformUsageDataAsync));

            var userCount = await _statisticsRepository.GetTotalNumberOfUserAsync();
            var searchCount = await _statisticsRepository.GetTotalNumberOfSearchesAsync();
            var ratesCount = await _statisticsRepository.GetTotalNumberOfRatesArticlesAsync();

            _logger?.LogInformation($"{nameof(GetPlatformUsageDataAsync)}: done");

            return new PlatformDataUsageResponse
            {
                PlatformUsers = userCount,
                PlatformSearches = searchCount,
                ArticleRates = ratesCount,
            };
        }

        public async Task<SearchesCountingResponse> GetTotalSearchesByUserIdAsync(Guid userId)
        {
            _logger?.LogInformation(nameof(GetTotalSearchesByUserIdAsync));

            var userSearchesCount = await _statisticsRepository.GetTotalNumberOfUserSearchesAsync(userId);

            _logger?.LogInformation($"{nameof(GetTotalSearchesByUserIdAsync)}: done");

            return new SearchesCountingResponse
            {
                UserSearches = userSearchesCount,
            };
        }
    }
}
