using System;
using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.Admin;
using CriThink.Server.Core.Interfaces;
using CriThink.Server.Web.Areas.BackOffice.ViewModels;

namespace CriThink.Server.Web.Facades
{
    public class TriggerLogServiceFacade : ITriggerLogServiceFacade
    {
        private readonly IDebunkingNewsService _debunkingNewsService;

        public TriggerLogServiceFacade(IDebunkingNewsService debunkingNewsService)
        {
            _debunkingNewsService = debunkingNewsService;
        }

        public async Task<TriggerLogsGetAllResponse> GetAllTriggerLogAsync(SimplePaginationViewModel viewModel) 
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));
            
            var request = new TriggerLogsGetAllRequest
            {
                PageIndex = viewModel.PageIndex,
                PageSize = viewModel.PageSize
            };

            return await _debunkingNewsService.GetAllTriggerLogsAsync(request).ConfigureAwait(false);
        }
    }
}