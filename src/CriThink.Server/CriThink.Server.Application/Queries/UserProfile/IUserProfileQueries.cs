using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.UserProfile;

namespace CriThink.Server.Application.Queries
{
    public interface IUserProfileQueries
    {
        /// <summary>
        /// Return user profile
        /// </summary>
        /// <returns></returns>
        Task<UserProfileGetResponse> GetUserProfileAsync();
    }
}
