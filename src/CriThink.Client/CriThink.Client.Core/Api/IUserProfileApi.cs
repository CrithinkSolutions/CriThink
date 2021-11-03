using System.Threading;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
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

        [Multipart]
        [Patch("/" + EndpointConstants.UserProfileUploadAvatar)]
        Task UpdateUserProfileAvatarAsync([AliasAs("formFile")] StreamPart streamPart, CancellationToken cancellationToken = default);

        [Get("/" + EndpointConstants.UserProfileRecentSearches)]
        Task<UserProfileGetAllRecentSearchResponse> GetUserRecentSearchesAsync(CancellationToken cancellationToken = default);
    }
}