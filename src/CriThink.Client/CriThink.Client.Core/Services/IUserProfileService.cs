using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Models.Identity;
using CriThink.Common.Endpoints.DTOs.UserProfile;

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
    }
}