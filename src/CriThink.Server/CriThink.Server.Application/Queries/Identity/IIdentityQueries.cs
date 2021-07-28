using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.Admin;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;

namespace CriThink.Server.Application.Queries
{
    public interface IIdentityQueries
    {
        /// <summary>
        /// Get the username availability. Returns true if username is not already
        /// </summary>
        /// <param name="username">User username</param>
        /// <returns>Returns true if the username if available, false if not</returns>
        Task<UsernameAvailabilityResponse> AnyUserByUsernameAsync(string username);

        /// <summary>
        /// Get all users
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns></returns>
        Task<UserGetAllResponse> GetAllUsersAsync(int pageIndex, int pageSize);

        /// <summary>
        /// Get all roles
        /// </summary>
        /// <returns></returns>
        Task<IList<RoleGetResponse>> GetAllRolesAsync();

        /// <summary>
        /// Get user details by id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<UserGetDetailsResponse> GetUserByIdAsync(Guid userId);
    }
}
