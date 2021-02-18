using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Services;
using CriThink.Client.Core.ViewModels.DebunkingNews;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using MvvmCross.Logging;
using MvvmCross.ViewModels;

namespace CriThink.Client.Core.ViewModels.NewsChecker
{
    public class NewsCheckerResultViewModel : BaseViewModel<Uri>, IDisposable
    {
        private readonly INewsSourceService _newsSourceService;
        private readonly IMvxViewModelLoader _mvxViewModelLoader;
        private readonly IMvxLog _log;

        private Uri _uri;
        private CancellationTokenSource _cancellationTokenSource;

        private bool _disposed;

        public NewsCheckerResultViewModel(INewsSourceService newsSourceService, IMvxViewModelLoader mvxViewModelLoader, IMvxLogProvider logProvider)
        {
            _newsSourceService = newsSourceService ?? throw new ArgumentNullException(nameof(newsSourceService));
            _mvxViewModelLoader = mvxViewModelLoader ?? throw new ArgumentNullException(nameof(mvxViewModelLoader));
            _log = logProvider?.GetLogFor<NewsCheckerResultViewModel>();
        }

        #region Properties

        public NewsCheckerResultDetailViewModel ResultDetailViewModel { get; private set; }

        public RelatedDebunkingNewsViewModel FirstRelatedDebunkingNews { get; private set; }

        public RelatedDebunkingNewsViewModel SecondRelatedDebunkingNews { get; private set; }

        public RelatedDebunkingNewsViewModel ThirdRelatedDebunkingNews { get; private set; }

        public RelatedDebunkingNewsViewModel FourthRelatedDebunkingNews { get; private set; }

        public RelatedDebunkingNewsViewModel FifthRelatedDebunkingNews { get; private set; }

        private string _title;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        #endregion

        public override void Prepare(Uri parameter)
        {
            if (parameter == null)
            {
                var argumentNullException = new ArgumentNullException(nameof(parameter));
                _log?.FatalException("The given paramter is null", argumentNullException);
                throw argumentNullException;
            }

            _uri = parameter;
            Title = _uri.Host;

            _log?.Info("User checks news source", _uri);
        }

        public override async Task Initialize()
        {
            await base.Initialize().ConfigureAwait(false);

            // TODO: Fix this
            IsLoading = false;

            InitializeChildrenViewModels();

            await SearchNewsSourceAsync().ConfigureAwait(true);
        }

        private void InitializeChildrenViewModels()
        {
            ResultDetailViewModel = LoadChildViewModel<NewsCheckerResultDetailViewModel>(_mvxViewModelLoader);
            FirstRelatedDebunkingNews = LoadChildViewModel<RelatedDebunkingNewsViewModel>(_mvxViewModelLoader);
            SecondRelatedDebunkingNews = LoadChildViewModel<RelatedDebunkingNewsViewModel>(_mvxViewModelLoader);
            ThirdRelatedDebunkingNews = LoadChildViewModel<RelatedDebunkingNewsViewModel>(_mvxViewModelLoader);
            FourthRelatedDebunkingNews = LoadChildViewModel<RelatedDebunkingNewsViewModel>(_mvxViewModelLoader);
            FifthRelatedDebunkingNews = LoadChildViewModel<RelatedDebunkingNewsViewModel>(_mvxViewModelLoader);
        }

        private async Task SearchNewsSourceAsync()
        {
            _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(45));

            try
            {
                var response = await _newsSourceService.SearchNewsSourceAsync(_uri, _cancellationTokenSource.Token)
                    .ConfigureAwait(true);

                if (response is null)
                    return;

                SetSearchResult(response);
                SetRelatedDebunkingNews(response);
            }
            catch (HttpRequestException)
            {
                await HandleUnknownResultAsync().ConfigureAwait(true);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void SetSearchResult(NewsSourceSearchWithDebunkingNewsResponse response)
        {
            var localizedClassificationText = LocalizedTextSource.GetText("ClassificationHeader");

            ResultDetailViewModel.Description = response.Description;
            ResultDetailViewModel.Classification = string.Format(CultureInfo.CurrentCulture, localizedClassificationText,
                response.Classification.ToString());
        }

        private void SetRelatedDebunkingNews(NewsSourceSearchWithDebunkingNewsResponse response)
        {
            if (!response.RelatedDebunkingNews.Any())
                return;

            FirstRelatedDebunkingNews.DebunkingNews = response.RelatedDebunkingNews.ElementAtOrDefault(0);
            SecondRelatedDebunkingNews.DebunkingNews = response.RelatedDebunkingNews.ElementAtOrDefault(1);
            ThirdRelatedDebunkingNews.DebunkingNews = response.RelatedDebunkingNews.ElementAtOrDefault(2);
            FourthRelatedDebunkingNews.DebunkingNews = response.RelatedDebunkingNews.ElementAtOrDefault(3);
            FifthRelatedDebunkingNews.DebunkingNews = response.RelatedDebunkingNews.ElementAtOrDefault(4);
        }

        private async Task HandleUnknownResultAsync()
        {
            await _newsSourceService.RegisterForNotificationAsync(_uri, _cancellationTokenSource.Token)
                .ConfigureAwait(true);

            ResultDetailViewModel.Classification = LocalizedTextSource.GetText("UnknownClassificationHeader");
            ResultDetailViewModel.Description = LocalizedTextSource.GetText("UnknownDescription");
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _cancellationTokenSource?.Dispose();
            }

            _disposed = true;
        }

        #endregion
    }
}
