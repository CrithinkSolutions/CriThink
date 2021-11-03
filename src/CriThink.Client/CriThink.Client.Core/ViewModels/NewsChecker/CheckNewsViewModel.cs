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

#pragma warning disable CA1056 // URI-like properties should not be strings
namespace CriThink.Client.Core.ViewModels.NewsChecker
{
    public class CheckNewsViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IUserProfileService _userProfileService;
        private readonly IUserDialogs _userDialogs;

        public CheckNewsViewModel(
            IMvxNavigationService navigationService,
            IUserProfileService userProfileService,
            IUserDialogs userDialogs)
        {
            _navigationService = navigationService ??
                throw new ArgumentNullException(nameof(navigationService));

            _userProfileService = userProfileService ??
                throw new ArgumentNullException(nameof(userProfileService));

            _userDialogs = userDialogs ??
                throw new ArgumentNullException(nameof(userDialogs));

            RecentNewsChecksCollection = new MvxObservableCollection<RecentNewsChecksModel>();
        }

        #region Properties

        public MvxObservableCollection<RecentNewsChecksModel> RecentNewsChecksCollection { get; }

        private string _newsUri;
        public string NewsUri
        {
            get => _newsUri;
            set
            {
                SetProperty(ref _newsUri, value);
                RaisePropertyChanged(() => SubmitUriCommand);
            }
        }

        #endregion

        #region Commands

        private IMvxAsyncCommand _submitUriCommand;
        public IMvxAsyncCommand SubmitUriCommand => _submitUriCommand ??= new MvxAsyncCommand(DoSubmitUriCommand, () =>
            !string.IsNullOrWhiteSpace(NewsUri));

        private IMvxAsyncCommand<RecentNewsChecksModel> _repeatSearchCommand;
        public IMvxAsyncCommand<RecentNewsChecksModel> RepeatSearchCommand => _repeatSearchCommand ??=
            new MvxAsyncCommand<RecentNewsChecksModel>(DoRepeatSearchCommand);

        private IMvxCommand _clearTextCommand;
        public IMvxCommand ClearTextCommand => _clearTextCommand ??= new MvxCommand(DoClearTextCommand);

        #endregion

        public override async Task Initialize()
        {
            await base.Initialize().ConfigureAwait(false);
            await UpdateLatestNewsChecksAsync().ConfigureAwait(false);
        }

        private async Task DoSubmitUriCommand(CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(NewsUri))
            {
                await ShowFormatMessageErrorAsync(cancellationToken).ConfigureAwait(true);
                return;
            }

            if (!NewsUri.IsUrl())
            {
                var title = LocalizedTextSource.GetText("MalformedUrlTitle");
                var message = LocalizedTextSource.GetText("MalformedUrlMessage");
                var ok = LocalizedTextSource.GetText("MalformedUrlConfirm");
                await ShowFormatMessageAsync(message, title, ok, cancellationToken);

                return;
            }

            await _navigationService
                .Navigate<WebViewNewsViewModel, string>(NewsUri, cancellationToken: cancellationToken)
                .ConfigureAwait(true);

            //await UpdateLatestNewsChecksAsync().ConfigureAwait(false);
        }

        private async Task DoRepeatSearchCommand(RecentNewsChecksModel model, CancellationToken cancellationToken)
        {
            NewsUri = model.NewsLink;
            await UpdateLatestNewsChecksAsync().ConfigureAwait(false);
        }

        private void DoClearTextCommand() => NewsUri = string.Empty;

        private async Task UpdateLatestNewsChecksAsync()
        {
            var modelCollection = await _userProfileService.GetUserRecentSearchesAsync().ConfigureAwait(false);
            if (modelCollection?.RecentSearches?.Any() == true)
            {
                RecentNewsChecksCollection.Clear();
                RecentNewsChecksCollection.AddRange(modelCollection.RecentSearches.Select(rs => new RecentNewsChecksModel(rs.Id, rs.NewsLink)));
            }
        }

        private Task ShowFormatMessageErrorAsync(CancellationToken cancellationToken)
        {
            var message = LocalizedTextSource.GetText("FormatErrorMessage");
            var ok = LocalizedTextSource.GetText("FormatErrorOk");

            return _userDialogs.AlertAsync(message, okText: ok, cancelToken: cancellationToken);
        }

        private Task ShowFormatMessageAsync(string message, string title, string ok, CancellationToken cancellationToken)
        {
            return _userDialogs.AlertAsync(message, title: title, okText: ok, cancelToken: cancellationToken);
        }
    }
}
