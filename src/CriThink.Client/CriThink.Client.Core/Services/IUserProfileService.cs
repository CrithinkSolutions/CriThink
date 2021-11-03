using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Models.Identity;
using CriThink.Common.Endpoints.DTOs.UserProfile;
using Refit;

namespace CriThink.Client.Core.Services
{
    public interface IUserProfileService
    {
        /// <summary>
        /// Get user profile info
        /// </summary>
        /// <param name="cancellationToken">(Optional) Cancellation token to cancel the operation</param>
        /// <returns></returns>
        Task<User> GetUserProfileAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Update user profile information
        /// </summary>
        /// <param name="request">Data to update</param>
        /// <param name="cancellationToken">(Optional) Cancellation token to cancel the operation</param>
        /// <returns></returns>
        Task UpdateUserProfileAsync(UserProfileUpdateRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update user profile avatar
        /// </summary>
        /// <param name="streamPart">The new avatar</param>
        /// <param name="cancellationToken">(Optional) Cancellation token to cancel the operation</param>
        /// <returns></returns>
        Task UpdateUserProfileAvatarAsync(StreamPart streamPart, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get recent news
        /// </summary>
        /// <param name="cancellationToken">(Optional) Cancellation token to cancel the operation</param>
        /// <returns></returns>
        Task<UserProfileGetAllRecentSearchResponse> GetUserRecentSearchesAsync(CancellationToken cancellationToken = default);
    }
}