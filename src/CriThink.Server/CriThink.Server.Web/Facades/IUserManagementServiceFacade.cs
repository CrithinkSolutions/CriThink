using System.Threading.Tasks;
using System.Collections.Generic;
using CriThink.Common.Endpoints.DTOs.Admin;
using CriThink.Server.Web.Areas.BackOffice.ViewModels;
using CriThink.Server.Web.Areas.BackOffice.ViewModels.UserManagement;

namespace CriThink.Server.Web.Facades
{
    public interface IUserManagementServiceFacade
    {
        Task<UserGetAllResponse> GetAllUserAsync(SimplePaginationViewModel viewModel);

        Task<IList<RoleGetResponse>> GetAllRolesAsync();

        Task CreateNewUserAsync(AddUserViewModel viewModel);
    }

}