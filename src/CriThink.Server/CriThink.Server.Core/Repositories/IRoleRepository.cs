using System.Collections.Generic;
using System.Threading.Tasks;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.QueryResults;
using Microsoft.AspNetCore.Identity;

namespace CriThink.Server.Core.Repositories
{
    public interface IRoleRepository
    {
        /// <summary>
        /// Gets all the roles
        /// </summary>
        /// <returns>Role list</returns>
        Task<IList<GetAllRolesQueryResult>> GetAllRolesAsync();

        /// <summary>
        /// Creates a new role
        /// </summary>
        /// <param name="userRole">The role to add</param>
        /// <returns>An <see cref="IdentityResult"/></returns>
        Task<IdentityResult> CreateNewRoleAsync(UserRole userRole);

        /// <summary>
        /// Deletes the given role
        /// </summary>
        /// <param name="userRole"></param>
        /// <returns>An <see cref="IdentityResult"/></returns>
        Task<IdentityResult> DeleteRoleAsync(UserRole userRole);

        /// <summary>
        /// Update the given role
        /// </summary>
        /// <param name="userRole">The role with the new properties</param>
        /// <returns>An <see cref="IdentityResult"/></returns>
        Task<IdentityResult> UpdateRoleAsync(UserRole userRole);

        /// <summary>
        /// Find a role with the given name
        /// </summary>
        /// <param name="roleName">The role name</param>
        /// <returns>An <see cref="IdentityResult"/></returns>
        Task<UserRole> FindRoleByNameAsync(string roleName);
    }
}
