using System;
using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.Admin;
using CriThink.Server.Core.Interfaces;
using CriThink.Server.Web.Areas.BackOffice.ViewModels;
using CriThink.Server.Web.Areas.BackOffice.ViewModels.DebunkingNews;

namespace CriThink.Server.Web.Facades
{
    public class DebunkingNewsServiceFacade : IDebunkingNewsServiceFacade
    {
        private readonly IDebunkingNewsService _debunkingNewsService;

        public DebunkingNewsServiceFacade(IDebunkingNewsService debunkingNewsService)
        {
            _debunkingNewsService = debunkingNewsService ?? throw new ArgumentNullException(nameof(debunkingNewsService));
        }

        public async Task AddDebunkingNewsAsync(AddNewsViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            var request = new DebunkingNewsAddRequest
            {
                Caption = viewModel.Caption,
                Link = viewModel.Link,
                Keywords = viewModel.Keywords,
                Title = viewModel.Title,
                ImageLink = viewModel.ImageLink
            };

            await _debunkingNewsService.AddDebunkingNewsAsync(request).ConfigureAwait(false);
        }

        public async Task DeleteDebunkingNewsAsync(SimpleDebunkingNewsViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            var request = new SimpleDebunkingNewsRequest
            {
                Id = viewModel.Id
            };

            await _debunkingNewsService.DeleteDebunkingNewsAsync(request).ConfigureAwait(false);
        }

        public async Task<DebunkingNewsGetAllResponse> GetAllDebunkingNewsAsync(SimplePaginationViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            var request = new DebunkingNewsGetAllRequest
            {
                PageIndex = viewModel.PageIndex,
                PageSize = viewModel.PageSize
            };

            return await _debunkingNewsService.GetAllDebunkingNewsAsync(request).ConfigureAwait(false);
        }

        public async Task<DebunkingNewsGetDetailsResponse> GetDebunkingNewsAsync(SimpleDebunkingNewsViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            var request = new DebunkingNewsGetRequest
            {
                Id = viewModel.Id
            };

            return await _debunkingNewsService.GetDebunkingNewsAsync(request).ConfigureAwait(false);
        }

        public async Task UpdateDebunkingNewsAsync(UpdateDebunkingNewsViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            var request = new DebunkingNewsUpdateRequest
            {
                Id = viewModel.Id,
                Title = viewModel.Title,
                Caption = viewModel.Caption,
                Link = viewModel.Link,
                ImageLink = viewModel.ImageLink,
            };

            await _debunkingNewsService.UpdateDebunkingNewsAsync(request).ConfigureAwait(false);
        }
    }
}
