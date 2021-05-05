using System.Threading;
using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.UserProfile;
using Refit;

namespace CriThink.Client.Core.Api
{
    public interface IUserProfileApi
    {
        [Get("/")]
        Task<UserProfileGetResponse> GetUserDetailsAsync(CancellationToken cancellationToken = default);

        [Patch("/")]
        Task UpdateUserProfileAsync([Body] UserProfileUpdateRequest request, CancellationToken cancellationToken = default);
    }
}