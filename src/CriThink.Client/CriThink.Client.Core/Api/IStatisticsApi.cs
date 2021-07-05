using System.Threading;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Common.Endpoints.DTOs.Statistics;
using Refit;

namespace CriThink.Client.Core.Api
{
    public interface IStatisticsApi
    {
        [Get("/" + EndpointConstants.StatisticsPlatform)]
        Task<PlatformDataUsageResponse> GetPlatformUsageDataAsync(CancellationToken cancellationToken = default);

        [Get("/" + EndpointConstants.StatisticsUserSearches)]
        Task<SearchesCountingResponse> GetTotalUserSearchesAsync(CancellationToken cancellationToken = default);
    }
}
