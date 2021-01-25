using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.Admin;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using CriThink.Server.Core.Interfaces;
using CriThink.Server.Web.Areas.BackOffice.ViewModels;
using CriThink.Server.Web.Areas.BackOffice.ViewModels.UserManagement;

namespace CriThink.Server.Web.Facades
{
    public class UserManagementServiceFacade : IUserManagementServiceFacade
    {
        private readonly IIdentityService _identityService;

        public UserManagementServiceFacade(IIdentityService identityService)
        {
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }

        public async Task<UserGetAllResponse> GetAllUserAsync(SimplePaginationViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            var request = new UserGetAllRequest
            {
                PageIndex = viewModel.PageIndex,
                PageSize = viewModel.PageSize
            };

            return await _identityService.GetAllUsersAsync(request).ConfigureAwait(false);
        }

        public async Task<IList<RoleGetResponse>> GetAllRolesAsync()
        {
            return await _identityService.GetRolesAsync().ConfigureAwait(false);
        }

        public async Task CreateNewUserAsync(AddUserViewModel viewModel) 
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            var request = new UserSignUpRequest
            {
                UserName = viewModel.UserName,
                Email = viewModel.Email,
                Password = viewModel.Password
            };
            
            await _identityService.CreateNewUserAsync(request).ConfigureAwait(false);
        }

        public async Task CreateNewAdminAsync(AddUserViewModel viewModel) 
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            var request = new AdminSignUpRequest
            {
                UserName = viewModel.UserName,
                Email = viewModel.Email,
                Password = viewModel.Password
            };
            
            await _identityService.CreateNewAdminAsync(request).ConfigureAwait(false);
        }

        public async Task DeleteUserAsync(SimpleUserManagementViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));
            
            var request = new UserGetRequest
            {
                UserId = viewModel.Id
            };

            await _identityService.DeleteUserAsync(request).ConfigureAwait(false);
        }

        public async Task SoftDeleteUserAsync(SimpleUserManagementViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));
            
            var request = new UserGetRequest
            {
                UserId = viewModel.Id
            };

            await _identityService.SoftDeleteUserAsync(request).ConfigureAwait(false);
        }

        public async Task<UserGetDetailsResponse> GetUserByIdAsync(SimpleUserManagementViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            var request = new UserGetRequest
            {
                UserId = viewModel.Id
            };

            return await _identityService.GetUserByIdAsync(request).ConfigureAwait(false);
        }

        public async Task UpdateUserAsync(UserUpdateViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            var request = new UserUpdateRequest
            {
                UserId = viewModel.Id,
                UserName = viewModel.UserName,
                IsEmailConfirmed = viewModel.IsEmailConfirmed,
                IsLockoutEnabled = viewModel.IsLockoutEnabled,
                LockoutEnd = viewModel.LockoutEnd
            };

            await _identityService.UpdateUserAsync(request).ConfigureAwait(false);
        }

        public async Task UpdateUserRoleAsync(UserRoleUpdateViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            var request = new UserRoleUpdateRequest
            {
                UserId = viewModel.Id,
                Role = viewModel.Role
            };

            await _identityService.UpdateUserRoleAsync(request).ConfigureAwait(false);
        }
    }
}