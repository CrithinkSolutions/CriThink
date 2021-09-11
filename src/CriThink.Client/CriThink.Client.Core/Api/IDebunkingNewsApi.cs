using System.Threading;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Common.Endpoints.DTOs.DebunkingNews;
using Refit;

namespace CriThink.Client.Core.Api
{
    public interface IDebunkingNewsApi
    {
        [Get("/" + EndpointConstants.DebunkingNewsGetAll)]
        Task<DebunkingNewsGetAllResponse> GetAllDebunkingNewsAsync(
            DebunkingNewsGetAllRequest request,
            [Header("Accept-Language")] string languages,
            CancellationToken cancellationToken = default);

        [Get("/")]
        Task<DebunkingNewsGetDetailsResponse> GetDebunkingNewsAsync(DebunkingNewsGetRequest request, CancellationToken cancellationToken = default);
    }
}
