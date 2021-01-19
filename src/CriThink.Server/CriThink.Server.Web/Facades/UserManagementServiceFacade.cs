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
    }
}