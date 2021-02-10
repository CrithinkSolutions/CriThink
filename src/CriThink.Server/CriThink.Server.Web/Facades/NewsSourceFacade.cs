using System;
using System.Linq;
using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using CriThink.Common.Endpoints.DTOs.NewsSource.Requests;
using CriThink.Common.Endpoints.DTOs.UnknownNewsSource.Requests;
using CriThink.Server.Core.Interfaces;
using CriThink.Server.Web.Areas.BackOffice.ViewModels.NewsSource;

namespace CriThink.Server.Web.Facades
{
    public class NewsSourceFacade : INewsSourceFacade
    {
        private readonly INewsSourceService _newsSourceService;
        private readonly IUnknownNewsSourceService _unknownNewsSourceService;

        public NewsSourceFacade(INewsSourceService newsSourceService, IUnknownNewsSourceService unknownNewsSourceService)
        {
            _newsSourceService = newsSourceService ?? throw new ArgumentNullException(nameof(newsSourceService));
            _unknownNewsSourceService = unknownNewsSourceService ?? throw new ArgumentNullException(nameof(unknownNewsSourceService));
        }

        public async Task<IndexViewModel> GetAllNewsSourcesAsync()
        {
            var response = await _newsSourceService.GetAllNewsSourcesAsync(NewsSourceGetAllFilterRequest.None).ConfigureAwait(false);

            return new IndexViewModel
            {
                NewsSources = response.Select(ToNewsSource),
            };
        }

        public async Task AddNewsSourceAsync(AddNewsSourceViewModel viewModel)
        {
            if (viewModel is null)
                throw new ArgumentNullException(nameof(viewModel));

            var request = new NewsSourceAddRequest
            {
                Uri = viewModel.Uri,
                Classification = ToRequestEnum(viewModel.Classification),
            };

            await _newsSourceService.AddSourceAsync(request).ConfigureAwait(false);
        }

        public async Task RemoveWhitelistNewsSourceAsync(Uri uri)
        {
            if (uri is null)
                throw new ArgumentNullException(nameof(uri));

            await _newsSourceService.RemoveGoodNewsSourceAsync(uri).ConfigureAwait(false);
        }

        public async Task RemoveBlacklistNewsSourceAsync(Uri uri)
        {
            if (uri is null)
                throw new ArgumentNullException(nameof(uri));

            await _newsSourceService.RemoveBadSourceAsync(uri).ConfigureAwait(false);
        }

        public async Task<NewsSourceViewModel> SearchNewsSourceAsync(Uri uri)
        {
            if (uri is null)
                throw new ArgumentNullException(nameof(uri));

            var response = await _newsSourceService.SearchNewsSourceAsync(uri).ConfigureAwait(false);

            return new NewsSourceViewModel
            {
                Uri = response.Description,
                Classification = ToViewModelEnum(response.Classification),
            };
        }

        public async Task TriggerIdentifiedNewsSourceAsync(TriggerIdentifiedNewsSourceViewModel viewModel)
        {
            if (viewModel is null)
                throw new ArgumentNullException(nameof(viewModel));

            var request = new TriggerUpdateForIdentifiedNewsSourceRequest
            {
                Uri = viewModel.Uri,
                Classification = ToRequestEnum(viewModel.Classification),
            };

            await _unknownNewsSourceService.TriggerUpdateForIdentifiedNewsSourceAsync(request).ConfigureAwait(false);
        }

        private static NewsSource ToNewsSource(NewsSourceGetAllResponse newsSource) => new NewsSource
        {
            Uri = newsSource.Uri,
            Classification = ToViewModelEnum(newsSource.NewsSourceClassification),
        };

        private static Classification ToViewModelEnum(NewsSourceClassification newsSourceClassification)
            => newsSourceClassification switch
            {
                NewsSourceClassification.Reliable => Classification.Reliable,
                NewsSourceClassification.Satirical => Classification.Satirical,
                NewsSourceClassification.Conspiracist => Classification.Conspiracist,
                NewsSourceClassification.FakeNews => Classification.FakeNews,
                _ => throw new NotImplementedException(nameof(ToViewModelEnum)),
            };

        private static NewsSourceClassification ToRequestEnum(Classification classification)
            => classification switch
            {
                Classification.Reliable => NewsSourceClassification.Reliable,
                Classification.Satirical => NewsSourceClassification.Satirical,
                Classification.Conspiracist => NewsSourceClassification.Conspiracist,
                Classification.FakeNews => NewsSourceClassification.FakeNews,
                _ => throw new NotImplementedException(nameof(ToRequestEnum)),
            };
    }
}