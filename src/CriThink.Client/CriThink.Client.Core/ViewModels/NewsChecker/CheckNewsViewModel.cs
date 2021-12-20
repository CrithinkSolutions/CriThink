using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using CriThink.Client.Core.Models.NewsChecker;
using CriThink.Client.Core.Services;
using CriThink.Common.Helpers;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace CriThink.Client.Core.ViewModels.NewsChecker
{
    public class CheckNewsViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IUserProfileService _userProfileService;
        private readonly IApplicationService _applicationService;
        private readonly IUserDialogs _userDialogs;

        public CheckNewsViewModel(
            IMvxNavigationService navigationService,
            IUserProfileService userProfileService,
            IApplicationService applicationService,
            IUserDialogs userDialogs)
        {
            _navigationService = navigationService ??
                throw new ArgumentNullException(nameof(navigationService));

            _userProfileService = userProfileService ??
                throw new ArgumentNullException(nameof(userProfileService));

            _applicationService = applicationService ??
                throw new ArgumentNullException(nameof(applicationService));

            _userDialogs = userDialogs ??
                throw new ArgumentNullException(nameof(userDialogs));

            RecentNewsChecksCollection = new MvxObservableCollection<RecentNewsChecksModel>();
        }

        #region Properties

        public MvxObservableCollection<RecentNewsChecksModel> RecentNewsChecksCollection { get; }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                RaisePropertyChanged(() => SubmitUriCommand);
            }
        }

        public bool IsFirstSearch => !IsLoading && !RecentNewsChecksCollection.Any();

        public bool RecentSearchesIsNotEmpty => !IsLoading && RecentNewsChecksCollection.Any();

        #endregion

        #region Commands

        private IMvxAsyncCommand _submitUriCommand;
        public IMvxAsyncCommand SubmitUriCommand => _submitUriCommand ??= new MvxAsyncCommand(DoSubmitUriCommand, () =>
            !string.IsNullOrWhiteSpace(SearchText));

        private IMvxAsyncCommand<RecentNewsChecksModel> _repeatSearchCommand;
        public IMvxAsyncCommand<RecentNewsChecksModel> RepeatSearchCommand => _repeatSearchCommand ??=
            new MvxAsyncCommand<RecentNewsChecksModel>(DoRepeatSearchCommand);

        private IMvxCommand _clearTextCommand;
        public IMvxCommand ClearTextCommand => _clearTextCommand ??= new MvxCommand(DoClearTextCommand);

        #endregion

        public override async Task Initialize()
        {
            await base.Initialize().ConfigureAwait(false);

            await Task.WhenAll(AskForReviewAsync(), UpdateLatestNewsChecksAsync());
        }

        private async Task DoSubmitUriCommand(CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                await ShowFormatMessageErrorAsync(cancellationToken).ConfigureAwait(true);
                return;
            }

            if (SearchText.IsUrl())
            {
                await _navigationService
                    .Navigate<WebViewNewsViewModel, string>(SearchText, cancellationToken: cancellationToken)
                    .ConfigureAwait(true);
            }
            else
            {
                await _navigationService
                    .Navigate<SearchTextResultViewModel, string>(SearchText, cancellationToken: cancellationToken)
                    .ConfigureAwait(true);
            }
        }

        private async Task DoRepeatSearchCommand(RecentNewsChecksModel model, CancellationToken cancellationToken)
        {
            SearchText = model.SearchedText;
            await UpdateLatestNewsChecksAsync().ConfigureAwait(false);
        }

        private void DoClearTextCommand() => SearchText = string.Empty;

        private async Task AskForReviewAsync()
        {
            if (await _applicationService.ShouldAskForStoreReviewAsync())
                await _applicationService.AskForStoreReviewAsync();
        }

        private async Task UpdateLatestNewsChecksAsync()
        {
            IsLoading = true;

            try
            {
                var modelCollection = await _userProfileService.GetUserRecentSearchesAsync().ConfigureAwait(false);
                if (modelCollection?.RecentSearches?.Any() == true)
                {
                    RecentNewsChecksCollection.Clear();

                    var recentNews = modelCollection.RecentSearches
                        .Select(rs => new RecentNewsChecksModel(
                            rs.Id,
                            rs.SearchedText,
                            rs.Title,
                            rs.Favicon,
                            rs.Timestamp.DateTime))
                        .ToList();

                    RecentNewsChecksCollection.AddRange(recentNews);
                }
            }
            finally
            {
                IsLoading = false;

                await RaisePropertyChanged(nameof(IsFirstSearch));
                await RaisePropertyChanged(nameof(RecentSearchesIsNotEmpty));
            }
        }

        private Task ShowFormatMessageErrorAsync(CancellationToken cancellationToken)
        {
            var message = LocalizedTextSource.GetText("FormatErrorMessage");
            var ok = LocalizedTextSource.GetText("FormatErrorOk");

            return _userDialogs.AlertAsync(message, okText: ok, cancelToken: cancellationToken);
        }
    }
}
