using System.Threading;
using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.Search;
using Refit;

namespace CriThink.Client.Core.Api
{
    public interface ISearchApi
    {
        [Get("/")]
        Task<SearchByTextResponse> SearchAsync([Query] string query, CancellationToken cancellationToken = default);
    }
}
