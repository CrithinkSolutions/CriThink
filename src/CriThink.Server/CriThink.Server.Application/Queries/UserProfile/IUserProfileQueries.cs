using System;
using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.UserProfile;

namespace CriThink.Server.Application.Queries
{
    public interface IUserProfileQueries
    {
        /// <summary>
        /// Return user profile
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns></returns>
        Task<UserProfileGetResponse> GetUserProfileByUserIdAsync(Guid userId);
    }
}
