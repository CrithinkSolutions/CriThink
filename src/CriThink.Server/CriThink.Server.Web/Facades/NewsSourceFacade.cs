using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CriThink.Common.Endpoints.DTOs.Admin;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using CriThink.Common.Endpoints.DTOs.UnknownNewsSource;
using CriThink.Server.Core.Exceptions;
using CriThink.Server.Core.Interfaces;
using CriThink.Server.Web.Areas.BackOffice.ViewModels;
using CriThink.Server.Web.Areas.BackOffice.ViewModels.NewsSource;

namespace CriThink.Server.Web.Facades
{
    public class NewsSourceFacade : INewsSourceFacade
    {
        private readonly INewsSourceService _newsSourceService;
        private readonly IUnknownNewsSourceService _unknownNewsSourceService;
        private readonly IMapper _mapper;

        public NewsSourceFacade(INewsSourceService newsSourceService, IUnknownNewsSourceService unknownNewsSourceService, IMapper mapper)
        {
            _newsSourceService = newsSourceService ?? throw new ArgumentNullException(nameof(newsSourceService));
            _unknownNewsSourceService = unknownNewsSourceService ?? throw new ArgumentNullException(nameof(unknownNewsSourceService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IndexViewModel> GetAllNewsSourcesAsync(SimplePaginationViewModel viewModel)
        {
            if (viewModel is null)
                throw new ArgumentNullException(nameof(viewModel));

            var request = new NewsSourceGetAllRequest
            {
                Filter = NewsSourceGetAllFilterRequest.None,
                PageIndex = viewModel.PageIndex,
                PageSize = viewModel.PageSize,
            };

            var response = await _newsSourceService.GetAllNewsSourcesAsync(request).ConfigureAwait(false);



            return new IndexViewModel
            {
                NewsSources = response.NewsSourcesCollection.Select(ToNewsSource),
                HasNextPage = response.HasNextPage,
            };
        }

        public async Task AddNewsSourceAsync(NewsSourceViewModel viewModel)
        {
            if (viewModel is null)
                throw new ArgumentNullException(nameof(viewModel));

            var newsSourceClassification = _mapper.Map<Classification, NewsSourceClassification>(viewModel.Classification);

            var request = new NewsSourceAddRequest
            {
                Uri = viewModel.Uri,
                Classification = newsSourceClassification,
            };

            await _newsSourceService.AddSourceAsync(request).ConfigureAwait(false);
        }

        public async Task RemoveNewsSourceAsync(Uri uri)
        {
            if (uri is null)
                throw new ArgumentNullException(nameof(uri));

            await _newsSourceService.RemoveNewsSourceAsync(uri).ConfigureAwait(false);
        }

        public async Task<NewsSourceViewModel> SearchNewsSourceAsync(Uri uri)
        {
            if (uri is null)
                throw new ArgumentNullException(nameof(uri));

            try
            {
                var response = await _newsSourceService.SearchNewsSourceAsync(uri).ConfigureAwait(false);

                var classification = _mapper.Map<NewsSourceClassification, Classification>(response.Classification);

                return new NewsSourceViewModel
                {
                    Uri = uri.ToString(),
                    Description = response.Description,
                    Classification = classification,
                };
            }
            catch (ResourceNotFoundException)
            {
                return null;
            }
        }

        public async Task<NotificationRequestGetAllResponse> GetPendingNotificationRequestsAsync(SimplePaginationViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            var request = new NewsSourceNotificationGetAllRequest
            {
                PageIndex = viewModel.PageIndex,
                PageSize = viewModel.PageSize
            };

            return await _unknownNewsSourceService.GetPendingNotificationRequestsAsync(request)
                .ConfigureAwait(false);
        }

        public async Task TriggerIdentifiedNewsSourceAsync(string uri, Classification classification)
        {
            var newsSourceClassification = _mapper.Map<Classification, NewsSourceClassification>(classification);

            var request = new TriggerUpdateForIdentifiedNewsSourceRequest
            {
                Uri = uri,
                Classification = newsSourceClassification,
            };

            await _unknownNewsSourceService.TriggerUpdateForIdentifiedNewsSourceAsync(request).ConfigureAwait(false);
        }

        public async Task<UnknownNewsSourceViewModel> GetUnknownNewsSourceAsync(Guid unknownNewsSourceId)
        {
            var result = await _unknownNewsSourceService.GetUnknownNewsSourceAsync(unknownNewsSourceId).ConfigureAwait(false);
            if (result is null)
                throw new ResourceNotFoundException($"Can't find a resource with id {unknownNewsSourceId}");

            var classification = _mapper.Map<NewsSourceClassification, Classification>(result.Classification);

            return new UnknownNewsSourceViewModel
            {
                Id = result.Id,
                Classification = classification,
                Uri = result.Uri,
            };
        }

        private NewsSource ToNewsSource(NewsSourceGetResponse newsSource)
        {
            var classification = _mapper.Map<NewsSourceClassification, Classification>(newsSource.NewsSourceClassification);
            return new NewsSource
            {
                Uri = newsSource.Uri,
                Classification = classification,
            };
        }
    }
}