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
        Task<UsersCountingResponse> GetUsersCountingAsync();
    }
}
