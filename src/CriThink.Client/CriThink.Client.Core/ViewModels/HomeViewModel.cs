using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using CriThink.Client.Core.Constants;
using CriThink.Client.Core.Models.Identity;
using CriThink.Client.Core.Services;
using CriThink.Client.Core.ViewModels.NewsChecker;
using CriThink.Client.Core.ViewModels.SpotFakeNews;
using CriThink.Client.Core.ViewModels.Statistics;
using CriThink.Client.Core.ViewModels.Users;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace CriThink.Client.Core.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IIdentityService _identityService;
        private readonly IUserDialogs _userDialogs;
        private readonly IUserProfileService _userProfileService;

        public HomeViewModel(
            IMvxNavigationService navigationService,
            IIdentityService identityService,
            IUserDialogs userDialogs,
            IUserProfileService userProfileService)
        {
            var tabs = new List<BaseBottomViewViewModel>
            {
                Mvx.IoCProvider.IoCConstruct<NewsCheckerViewModel>(),
                Mvx.IoCProvider.IoCConstruct<HomeStatisticsViewModel>(),
                Mvx.IoCProvider.IoCConstruct<SpotFakeNewsHomeViewModel>(),
                Mvx.IoCProvider.IoCConstruct<AboutViewModel>(),
            };

            BottomViewTabs = tabs;
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _userDialogs = userDialogs ?? throw new ArgumentNullException(nameof(userDialogs));
            _userProfileService = userProfileService ?? throw new ArgumentNullException(nameof(userProfileService));
        }

        #region Properties

        private User _user;
        public User User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public List<BaseBottomViewViewModel> BottomViewTabs { get; }

        private IMvxCommand<string> _bottomNavigationItemSelectedCommand;
        public IMvxCommand<string> BottomNavigationItemSelectedCommand => _bottomNavigationItemSelectedCommand ??= new MvxCommand<string>(DoBottomNavigationItemSelectedCommand);

        private IMvxAsyncCommand<string> _navigateToNewsCheckResultViewModel;
        public IMvxAsyncCommand<string> NavigateToNewsCheckResultViewModel => _navigateToNewsCheckResultViewModel ??=
            new MvxAsyncCommand<string>(DoNavigateToNewsCheckResultViewModel);

        #endregion

        public override async Task Initialize()
        {

            await base.Initialize().ConfigureAwait(false);
            var user = await _identityService.GetLoggedUserAccessAsync().ConfigureAwait(false);
            if (user is null)
            {
                await _navigationService.Navigate<SignUpViewModel>(
                    new MvxBundle(new Dictionary<string, string>
                    {
                        {MvxBundleConstaints.ClearBackStack, ""}
                    })).ConfigureAwait(true);
            }
            var userProfile = await _userProfileService.GetUserProfileAsync().ConfigureAwait(false);
            if (userProfile != null)
            {
                User = userProfile;
            }
        }

        private void DoBottomNavigationItemSelectedCommand(string tabId)
        {
            foreach (var item in BottomViewTabs.Where(item => tabId == item.TabId))
            {
                _navigationService.Navigate(item);
                break;
            }
        }

        private async Task DoNavigateToNewsCheckResultViewModel(string input, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                await ShowFormatMessageErrorAsync(cancellationToken).ConfigureAwait(true);
                return;
            }

            await _navigationService
                .Navigate<WebViewNewsViewModel, string>(input, cancellationToken: cancellationToken)
                .ConfigureAwait(true);
        }

        private Task ShowFormatMessageErrorAsync(CancellationToken cancellationToken)
        {
            var message = LocalizedTextSource.GetText("FormatErrorMessage");
            var ok = LocalizedTextSource.GetText("FormatErrorOk");

            return _userDialogs.AlertAsync(message, okText: ok, cancelToken: cancellationToken);
        }
    }
}
