using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.Statistics;

namespace CriThink.Server.Core.Interfaces
{
    public interface IStatisticsService
    {
        /// <summary>
        /// Returns the number of users not marked as deleted
        /// </summary>
        /// <returns></returns>
        Task<PlatformDataUsageResponse> GetPlatformDataUsageAsync();

        /// <summary>
        /// Returns the number of total searches of the current user
        /// </summary>
        /// <returns></returns>
        Task<SearchesCountingResponse> GetUserTotalSearchesAsync();
    }
}
