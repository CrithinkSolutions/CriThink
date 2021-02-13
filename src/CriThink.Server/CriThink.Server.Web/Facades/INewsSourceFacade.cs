using System;
using System.Threading.Tasks;
using CriThink.Server.Web.Areas.BackOffice.ViewModels;
using CriThink.Server.Web.Areas.BackOffice.ViewModels.NewsSource;

namespace CriThink.Server.Web.Facades
{
    public interface INewsSourceFacade
    {
        Task<IndexViewModel> GetAllNewsSourcesAsync(SimplePaginationViewModel viewModel);

        Task AddNewsSourceAsync(NewsSourceViewModel viewModel);

        Task RemoveNewsSourceAsync(Uri uri);

        Task<NewsSourceViewModel> SearchNewsSourceAsync(Uri uri);

        Task TriggerIdentifiedNewsSourceAsync(string uri, Classification classification);

        Task<UnknownNewsSourceViewModel> GetUnknownNewsSourceAsync(Guid unknownNewsSourceId);
    }
}