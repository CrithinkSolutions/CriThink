using System;
using System.Linq;
using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using CriThink.Server.Core.Exceptions;
using CriThink.Server.Core.Interfaces;
using CriThink.Server.Web.Areas.BackOffice.ViewModels;
using CriThink.Server.Web.Areas.BackOffice.ViewModels.NewsSource;

namespace CriThink.Server.Web.Facades
{
    public class NewsSourceFacade : INewsSourceFacade
    {
        private readonly INewsSourceService _newsSourceService;

        public NewsSourceFacade(INewsSourceService newsSourceService)
        {
            _newsSourceService = newsSourceService ?? throw new ArgumentNullException(nameof(newsSourceService));
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

            var request = new NewsSourceAddRequest
            {
                Uri = viewModel.Uri,
                Classification = ToRequestEnum(viewModel.Classification),
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

                return new NewsSourceViewModel
                {
                    Uri = uri.ToString(),
                    Description = response.Description,
                    Classification = ToViewModelEnum(response.Classification),
                };
            }
            catch (ResourceNotFoundException)
            {
                return null;
            }
        }

        private static NewsSource ToNewsSource(NewsSourceGetResponse newsSource) =>
            new NewsSource
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