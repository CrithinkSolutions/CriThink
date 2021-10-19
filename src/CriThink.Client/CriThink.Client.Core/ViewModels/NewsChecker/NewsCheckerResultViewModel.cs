﻿using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Models.NewsChecker;
using CriThink.Client.Core.Services;
using CriThink.Client.Core.ViewModels.DebunkingNews;
using CriThink.Common.Endpoints.DTOs.DebunkingNews;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace CriThink.Client.Core.ViewModels.NewsChecker
{
    public class NewsCheckerResultViewModel : BaseViewModel<NewsCheckerResultModel>
    {
        private readonly INewsSourceService _newsSourceService;
        private readonly IMvxNavigationService _navigationService;
        private readonly ILogger<NewsCheckerResultViewModel> _logger;

        public NewsCheckerResultViewModel(
            INewsSourceService newsSourceService,
            ILogger<NewsCheckerResultViewModel> logger,
            IMvxNavigationService navigationService)
        {
            _newsSourceService = newsSourceService ??
                throw new ArgumentNullException(nameof(newsSourceService));

            _navigationService = navigationService ??
                throw new ArgumentNullException(nameof(navigationService));

            _logger = logger;

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
        private string _classificationTitle;
        public string ClassificationTitle
        {
            get => _classificationTitle;
            set => SetProperty(ref _classificationTitle, value);
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

        private bool _isSubscribed;
        public bool IsSubscribed
        {
            get => _isSubscribed;
            set
            {
                SetProperty(ref _isSubscribed, value);
                Task.Run(async () => await NotificationSubscribedAsync());
            }
        }

        private NewsCheckerResultModel _newsCheckerResultModel;
        public NewsCheckerResultModel NewsCheckerResultModel
        {
            get => _newsCheckerResultModel;
            set => SetProperty(ref _newsCheckerResultModel, value);
        }

        private IMvxCommand<NewsSourceRelatedDebunkingNewsResponse> _debunkingNewsSelectedCommand;
        public IMvxCommand<NewsSourceRelatedDebunkingNewsResponse> DebunkingNewsSelectedCommand =>
            _debunkingNewsSelectedCommand ??= new MvxAsyncCommand<NewsSourceRelatedDebunkingNewsResponse>(DoDebunkingNewsSelectedCommand);


        #endregion

        public override void Prepare(NewsCheckerResultModel parameter)
        {
            if (parameter == null)
            {
                var argumentNullException = new ArgumentNullException(nameof(parameter));
                _logger?.LogCritical(argumentNullException, "The given paramter is null");
                throw argumentNullException;
            }

            _newsCheckerResultModel = parameter;

        }

        public override async Task Initialize()
        {
            await base.Initialize().ConfigureAwait(false);
            await SetNewsSourceAsync().ConfigureAwait(true);
        }

        private async Task SetNewsSourceAsync()
        {
            if (NewsCheckerResultModel.IsUnknownResult)
            {
                await HandleUnknownResultAsync(NewsCheckerResultModel.NewsLink).ConfigureAwait(true);
            }
            else
            {
                SetSearchResult(NewsCheckerResultModel.NewsSourcePostAnswersResponse);
                SetRelatedDebunkingNews(NewsCheckerResultModel.NewsSourcePostAnswersResponse);
            }
        }

        private void SetSearchResult(NewsSourcePostAnswersResponse response)
        {
            var localizedClassificationText = LocalizedTextSource.GetText("ClassificationHeader");

            ClassificationTitle = LocalizedTextSource.GetText("ResponseTitle");

            Description = response.Description;
            Classification = string.Format(CultureInfo.CurrentCulture, localizedClassificationText, response.Classification.ToString());

            ResultImage = response.Classification switch
            {
                NewsSourceAuthenticityDto.Conspiracist => "result_conspiracy.svg",
                NewsSourceAuthenticityDto.FakeNews => "result_fakenews.svg",
                NewsSourceAuthenticityDto.Reliable => "result_reliable.svg",
                NewsSourceAuthenticityDto.Satirical => "result_satirical.svg",
                NewsSourceAuthenticityDto.SocialMedia => "result_socialmedia.svg",
                NewsSourceAuthenticityDto.Suspicious => "result_suspicious.svg",
                _ => "result_suspicious.svg"
            };
        }

        private void SetRelatedDebunkingNews(NewsSourcePostAnswersResponse response)
        {
            if (!response.RelatedDebunkingNews.Any())
                return;

            Feed.AddRange(response.RelatedDebunkingNews);
            RaisePropertyChanged(nameof(HasRelatedDebunkingNews));
        }

        private async Task HandleUnknownResultAsync(string newsLink)
        {
            await _newsSourceService.RegisterForNotificationAsync(newsLink)
                .ConfigureAwait(true);

            IsSubscribed = true;
            ClassificationTitle = LocalizedTextSource.GetText("UnknownClassificatioHeader");
            Classification = LocalizedTextSource.GetText("UnknownClassification");
            Description = LocalizedTextSource.GetText("UnknownDescription");
        }

        private async Task NotificationSubscribedAsync()
        {
            try
            {
                if (IsSubscribed)
                {
                    await _newsSourceService.UnregisterForNotificationAsync(
                        NewsCheckerResultModel.NewsLink);
                }
                else
                {
                    await _newsSourceService.RegisterForNotificationAsync(
                        NewsCheckerResultModel.NewsLink);
                }

                InvokeOnMainThread(() => IsSubscribed = !IsSubscribed);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        private async Task DoDebunkingNewsSelectedCommand(
            NewsSourceRelatedDebunkingNewsResponse selectedResponse,
            CancellationToken cancellationToken)
        {
            _logger?.LogInformation("User opens debunking news", selectedResponse.NewsLink);

            var response = new DebunkingNewsGetResponse
            {
                Title = selectedResponse.Title,
                NewsLink = selectedResponse.NewsLink,
                Id = selectedResponse.Id,
                NewsImageLink = selectedResponse.NewsImageLink,
                Publisher = selectedResponse.Publisher,
            };

            await _navigationService.Navigate<DebunkingNewsDetailsViewModel, DebunkingNewsGetResponse>(
                response,
                cancellationToken: cancellationToken)
                .ConfigureAwait(true);
        }
    }
}
