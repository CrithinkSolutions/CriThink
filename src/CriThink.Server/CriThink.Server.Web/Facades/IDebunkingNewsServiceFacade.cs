using System.Collections.Generic;
using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.Admin;
using CriThink.Server.Web.Areas.BackOffice.ViewModels.DebunkingNews;
using CriThink.Server.Web.Areas.BackOffice.ViewModels.Shared;

namespace CriThink.Server.Web.Facades
{
    public interface IDebunkingNewsServiceFacade
    {
        Task AddDebunkingNewsAsync(AddNewsViewModel viewModel);

        Task DeleteDebunkingNewsAsync(SimpleDebunkingNewsViewModel viewModel);

        Task<IList<DebunkingNewsGetAllResponse>> GetAllDebunkingNewsAsync(SimplePaginationViewModel viewModel);
    }
}
