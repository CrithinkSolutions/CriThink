using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.Admin;
using CriThink.Server.Core.Interfaces;
using CriThink.Server.Web.Areas.BackOffice.ViewModels.DebunkingNews;
using CriThink.Server.Web.Areas.BackOffice.ViewModels.Shared;

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
                Title = viewModel.Title
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

        public async Task<IList<DebunkingNewsGetAllResponse>> GetAllDebunkingNewsAsync(SimplePaginationViewModel viewModel)
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
    }
}
