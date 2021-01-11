using System;
using System.Threading.Tasks;
using CriThink.Client.Core.Services;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace CriThink.Client.Core.ViewModels.Users
{
    public class WelcomeLoginSignInViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IPlatformService _platformService;

        public WelcomeLoginSignInViewModel(IMvxNavigationService navigationService, IPlatformService platformService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _platformService = platformService ?? throw new ArgumentNullException(nameof(platformService));
        }

        #region Commands

        private IMvxAsyncCommand _navigateToSignInViewCommand;
        public IMvxAsyncCommand NavigateToSignInViewCommand => _navigateToSignInViewCommand ??= new MvxAsyncCommand(DoNavigateToSignInViewCommand);

        private IMvxAsyncCommand _navigateToLoginViewCommand;
        public IMvxAsyncCommand NavigateToLoginViewCommand => _navigateToLoginViewCommand ??= new MvxAsyncCommand(DoNavigateToLoginViewCommand);

        private IMvxCommand _openFacebookPageCommand;
        public IMvxCommand OpenFacebookPageCommand => _openFacebookPageCommand ??= new MvxCommand(DoOpenFacebookPageCommand);

        private IMvxCommand _openInstagramPageCommand;
        public IMvxCommand OpenInstagramPageCommand => _openInstagramPageCommand ??= new MvxCommand(DoOpenInstagramPageCommand);

        private IMvxCommand _openLinkedInPageCommand;
        public IMvxCommand OpenLinkedInPageCommand => _openLinkedInPageCommand ??= new MvxCommand(DoOpenLinkedInPageCommand);

        #endregion

        private async Task DoNavigateToSignInViewCommand()
        {
            await _navigationService.Navigate<SignUpViewModel>().ConfigureAwait(true);
        }

        private async Task DoNavigateToLoginViewCommand()
        {
            await _navigationService.Navigate<LoginViewModel>().ConfigureAwait(true);
        }

        private void DoOpenFacebookPageCommand()
        {
            _platformService.OpenCriThinkFacebookPage();
        }

        private void DoOpenInstagramPageCommand()
        {
            _platformService.OpenCriThinkInstagramProfile();
        }

        private void DoOpenLinkedInPageCommand()
        {
            _platformService.OpenCriThinkLinkedInProfile();
        }
    }
}
