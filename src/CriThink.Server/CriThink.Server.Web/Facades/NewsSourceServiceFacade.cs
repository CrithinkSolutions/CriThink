using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CriThink.Server.Core.Interfaces;
using CriThink.Common.Endpoints.DTOs.NewsSource;

namespace CriThink.Server.Web.Facades
{
    public class NewsSourceServiceFacade : INewsSourceServiceFacade
    {
        private readonly INewsSourceService _newsSourceService;

        public NewsSourceServiceFacade(INewsSourceService newsSourceService)
        {
            _newsSourceService = newsSourceService ?? throw new ArgumentNullException(nameof(newsSourceService));
        }

        public async Task<IList<NewsSourceGetAllResponse>> GetAllNewsSource()
        {
            return await _newsSourceService.GetAllNewsSourcesAsync(0).ConfigureAwait(false);
        }
    }
}