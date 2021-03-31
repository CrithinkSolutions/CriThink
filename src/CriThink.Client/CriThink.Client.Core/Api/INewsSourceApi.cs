using System.Threading;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using CriThink.Common.Endpoints.DTOs.UnknownNewsSource;
using Refit;

namespace CriThink.Client.Core.Api
{
    public interface INewsSourceApi
    {
        [Post("/" + EndpointConstants.NewsSourceRegisterForNotification)]
        Task RegisterForNotificationAsync(NewsSourceNotificationForUnknownDomainRequest request, CancellationToken cancellationToken = default);

        [Get("/" + EndpointConstants.NewsSourceSearch)]
        Task<NewsSourceSearchWithDebunkingNewsResponse> SearchNewsSourceAsync([Query] NewsSourceSearchRequest request, CancellationToken cancellationToken = default);
    }
}