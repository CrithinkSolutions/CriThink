using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.Admin;
using CriThink.Server.Web.Areas.BackOffice.ViewModels;
using CriThink.Server.Web.Areas.BackOffice.ViewModels.DebunkingNews;

namespace CriThink.Server.Web.Facades
{
    public interface IDebunkingNewsServiceFacade
    {
        Task AddDebunkingNewsAsync(AddNewsViewModel viewModel);

        Task DeleteDebunkingNewsAsync(SimpleDebunkingNewsViewModel viewModel);

        Task<DebunkingNewsGetAllResponse> GetAllDebunkingNewsAsync(SimplePaginationViewModel viewModel);

        Task<DebunkingNewsGetDetailsResponse> GetDebunkingNewsAsync(SimpleDebunkingNewsViewModel viewModel);

        Task UpdateDebunkingNewsAsync(EditDebunkingNewsViewModel viewModel);
    }
}
