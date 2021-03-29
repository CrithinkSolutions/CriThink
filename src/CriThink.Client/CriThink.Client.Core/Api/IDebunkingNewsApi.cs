using System.Threading;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Common.Endpoints.DTOs.Admin;
using Refit;

namespace CriThink.Client.Core.Api
{
    public interface IDebunkingNewsApi
    {
        [Get("/" + EndpointConstants.DebunkingNewsGetAll)]
        Task<DebunkingNewsGetAllResponse> GetAllDebunkingNewsAsync(DebunkingNewsGetAllRequest request, CancellationToken cancellationToken = default);

        [Get("/")]
        Task<DebunkingNewsGetDetailsResponse> GetDebunkingNewsAsync(DebunkingNewsGetRequest request, CancellationToken cancellationToken = default);
    }
}
