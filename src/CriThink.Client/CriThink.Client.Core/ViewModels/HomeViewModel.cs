﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using CriThink.Client.Core.Constants;
using CriThink.Client.Core.Services;
using CriThink.Client.Core.ViewModels.Games;
using CriThink.Client.Core.ViewModels.NewsChecker;
using CriThink.Client.Core.ViewModels.SpotFakeNews;
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

        public HomeViewModel(IMvxNavigationService navigationService, IIdentityService identityService, IUserDialogs userDialogs)
        {
            var tabs = new List<BaseBottomViewViewModel>
            {
                Mvx.IoCProvider.IoCConstruct<NewsCheckerViewModel>(),
                Mvx.IoCProvider.IoCConstruct<SpotFakeNewsHomeViewModel>(),
                Mvx.IoCProvider.IoCConstruct<HomeGameViewModel>(),
                Mvx.IoCProvider.IoCConstruct<AboutViewModel>(),
            };

            BottomViewTabs = tabs;

            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _userDialogs = userDialogs ?? throw new ArgumentNullException(nameof(userDialogs));
        }

        #region Properties

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

            var user = await _identityService.GetLoggedUserAsync().ConfigureAwait(false);
            if (user is null)
            {
                await _navigationService.Navigate<SignUpViewModel>(
                    new MvxBundle(new Dictionary<string, string>
                    {
                        {MvxBundleConstaints.ClearBackStack, ""}
                    })).ConfigureAwait(true);
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
                .Navigate<NewsCheckerResultViewModel, string>(input, cancellationToken: cancellationToken)
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
