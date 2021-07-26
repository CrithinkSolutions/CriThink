using System;
using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.Statistics;
using CriThink.Server.Core.Repositories;
using CriThink.Server.Infrastructure.ExtensionMethods;
using Microsoft.AspNetCore.Http;

namespace CriThink.Server.Application.Queries
{
    internal class StatisticsQueries : IStatisticsQueries
    {
        private readonly IStatisticsRepository _statisticsRepository;
        private readonly IHttpContextAccessor _context;

        public StatisticsQueries(IStatisticsRepository statisticsRepository, IHttpContextAccessor context)
        {
            _statisticsRepository = statisticsRepository ??
                throw new ArgumentNullException(nameof(statisticsRepository));

            _context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public async Task<PlatformDataUsageResponse> GetPlatformUsageDataAsync()
        {
            var userCount = await _statisticsRepository.GetTotalNumberOfUserAsync();
            var searchCount = await _statisticsRepository.GetTotalNumberOfSearchesAsync();
            var ratesCount = await _statisticsRepository.GetTotalNumberOfRatesArticlesAsync();

            return new PlatformDataUsageResponse
            {
                PlatformUsers = userCount,
                PlatformSearches = searchCount,
                ArticleRates = ratesCount,
            };
        }

        public async Task<SearchesCountingResponse> GetUserTotalSearchesAsync()
        {
            var userId = _context.HttpContext.User.GetId();

            var userSearchesCount = await _statisticsRepository.GetTotalNumberOfUserSearchesAsync(userId);

            return new SearchesCountingResponse
            {
                UserSearches = userSearchesCount,
            };
        }
    }
}
