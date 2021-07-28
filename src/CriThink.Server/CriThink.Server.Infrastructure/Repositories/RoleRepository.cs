using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.Admin;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CriThink.Server.Infrastructure.Repositories
{
    internal class RoleRepository : IRoleRepository
    {
        private readonly RoleManager<UserRole> _roleManager;

        public RoleRepository(RoleManager<UserRole> roleManager)
        {
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }

        public async Task<IList<RoleGetResponse>> GetAllRolesAsync()
        {
            var allRoles = await _roleManager.Roles
                .Select(r => new RoleGetResponse
                {
                    Name = r.Name,
                    Id = r.Id.ToString()
                })
                .ToListAsync()
                .ConfigureAwait(false);

            return allRoles;
        }

        public Task<IdentityResult> CreateNewRoleAsync(UserRole userRole)
        {
            return _roleManager.CreateAsync(userRole);
        }

        public Task<IdentityResult> DeleteRoleAsync(UserRole userRole)
        {
            return _roleManager.DeleteAsync(userRole);
        }

        public Task<IdentityResult> UpdateRoleAsync(UserRole userRole)
        {
            return _roleManager.UpdateAsync(userRole);
        }

        public async Task<UserRole> FindRoleByNameAsync(string roleName)
        {
            if (!string.IsNullOrWhiteSpace(roleName))
                return await _roleManager.FindByNameAsync(roleName).ConfigureAwait(false);

            return null;
        }
    }
}
