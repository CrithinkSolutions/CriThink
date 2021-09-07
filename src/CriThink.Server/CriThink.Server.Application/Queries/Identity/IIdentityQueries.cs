using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using CriThink.Server.Application.Administration.ViewModels;

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
        /// <param name="pageSize">Page size</param>
        /// <param name="pageIndex">Page index</param>
        /// <returns></returns>
        Task<UserGetAllViewModel> GetAllUsersAsync(int pageSize, int pageIndex);

        /// <summary>
        /// Get all roles
        /// </summary>
        /// <returns></returns>
        Task<IList<RoleGetViewModel>> GetAllRolesAsync();

        /// <summary>
        /// Get user details by id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<UserGetDetailsViewModel> GetUserByIdAsync(Guid userId);
    }
}
