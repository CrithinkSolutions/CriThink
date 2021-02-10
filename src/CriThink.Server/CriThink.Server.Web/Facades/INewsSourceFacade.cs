using System;
using System.Threading.Tasks;
using CriThink.Server.Web.Areas.BackOffice.ViewModels.NewsSource;

namespace CriThink.Server.Web.Facades
{
    public interface INewsSourceFacade
    {
        Task<IndexViewModel> GetAllNewsSourcesAsync();

        Task AddNewsSourceAsync(AddNewsSourceViewModel viewModel);

        Task RemoveWhitelistNewsSourceAsync(Uri uri);

        Task RemoveBlacklistNewsSourceAsync(Uri uri);

        Task<NewsSourceViewModel> SearchNewsSourceAsync(Uri uri);

        Task TriggerIdentifiedNewsSourceAsync();
    }
}