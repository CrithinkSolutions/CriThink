using System.Threading;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using Refit;

namespace CriThink.Client.Core.Api
{
    public interface IAuthorizedIdentityApi
    {
        [Delete("/" + EndpointConstants.IdentityDeleteUser)]
        Task<UserSoftDeletionResponse> DeleteUserAsync(CancellationToken cancellationToken = default);
    }
}
