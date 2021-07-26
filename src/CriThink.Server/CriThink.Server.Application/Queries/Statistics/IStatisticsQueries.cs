using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.Statistics;

namespace CriThink.Server.Application.Queries
{
    public interface IStatisticsQueries
    {
        /// <summary>
        /// Returns the public platform usage data
        /// </summary>
        /// <returns>The total number of users, the total
        /// number of searches, the total number of rates
        /// given to articles</returns>
        Task<PlatformDataUsageResponse> GetPlatformUsageDataAsync();

        /// <summary>
        /// Returns the number of total searches of the current user
        /// </summary>
        /// <returns></returns>
        Task<SearchesCountingResponse> GetUserTotalSearchesAsync();
    }
}
