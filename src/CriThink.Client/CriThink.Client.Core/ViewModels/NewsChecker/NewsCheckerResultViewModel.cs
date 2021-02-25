﻿using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Services;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.ViewModels;

namespace CriThink.Client.Core.ViewModels.NewsChecker
{
    public class NewsCheckerResultViewModel : BaseViewModel<Uri>
    {
        private readonly INewsSourceService _newsSourceService;
        private readonly IDebunkingNewsService _debunkingNewsService;
        private readonly IMvxLog _log;

        private Uri _uri;
        private CancellationTokenSource _cancellationTokenSource;

        public NewsCheckerResultViewModel(INewsSourceService newsSourceService, IMvxLogProvider logProvider, IDebunkingNewsService debunkingNewsService)
        {
            _newsSourceService = newsSourceService ?? throw new ArgumentNullException(nameof(newsSourceService));
            _debunkingNewsService = debunkingNewsService ?? throw new ArgumentNullException(nameof(debunkingNewsService));
            _log = logProvider?.GetLogFor<NewsCheckerResultViewModel>();

            Feed = new MvxObservableCollection<NewsSourceRelatedDebunkingNewsResponse>();
        }

        #region Properties

        public bool HasRelatedDebunkingNews => Feed.Any();

        public MvxObservableCollection<NewsSourceRelatedDebunkingNewsResponse> Feed { get; }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private string _classification;
        public string Classification
        {
            get => _classification;
            set => SetProperty(ref _classification, value);
        }

        private string _resultImage;
        public string ResultImage
        {
            get => _resultImage;
            set => SetProperty(ref _resultImage, value);
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private IMvxCommand<NewsSourceRelatedDebunkingNewsResponse> _debunkingNewsSelectedCommand;
        public IMvxCommand<NewsSourceRelatedDebunkingNewsResponse> DebunkingNewsSelectedCommand =>
            _debunkingNewsSelectedCommand ??= new MvxAsyncCommand<NewsSourceRelatedDebunkingNewsResponse>(DoDebunkingNewsSelectedCommand);

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

        public override void ViewDestroy(bool viewFinishing = true)
        {
            base.ViewDestroy(viewFinishing);
            _cancellationTokenSource?.Dispose();
        }

        public override async Task Initialize()
        {
            await base.Initialize().ConfigureAwait(false);

            // TODO: Fix this
            IsLoading = false;

            await SearchNewsSourceAsync().ConfigureAwait(true);
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

            Description = response.Description;
            Classification = string.Format(CultureInfo.CurrentCulture, localizedClassificationText, response.Classification.ToString());

            ResultImage = response.Classification switch
            {
                NewsSourceClassification.Conspiracist => "result_conspiracy.svg",
                NewsSourceClassification.FakeNews => "result_fakenews.svg",
                NewsSourceClassification.Reliable => "result_reliable.svg",
                NewsSourceClassification.Satirical => "result_satirical.svg",
                NewsSourceClassification.SocialMedia => "result_conspiracy.svg", // TODO: Change
                NewsSourceClassification.Suspicious => "result_conspiracy.svg", // TODO: change
                _ => "result_conspiracy.svg"
            };
        }

        private void SetRelatedDebunkingNews(NewsSourceSearchWithDebunkingNewsResponse response)
        {
            if (!response.RelatedDebunkingNews.Any())
                return;

            Feed.AddRange(response.RelatedDebunkingNews);
            RaisePropertyChanged(nameof(HasRelatedDebunkingNews));
        }

        private async Task HandleUnknownResultAsync()
        {
            await _newsSourceService.RegisterForNotificationAsync(_uri, _cancellationTokenSource.Token)
                .ConfigureAwait(true);

            Classification = LocalizedTextSource.GetText("UnknownClassificationHeader");
            Description = LocalizedTextSource.GetText("UnknownDescription");
        }

        private async Task DoDebunkingNewsSelectedCommand(NewsSourceRelatedDebunkingNewsResponse selectedResponse, CancellationToken cancellationToken)
        {
            await _debunkingNewsService.OpenDebunkingNewsInBrowser(selectedResponse.NewsLink).ConfigureAwait(false);
            _log?.Info("User opens debunking news", selectedResponse.NewsLink);
        }
    }
}
