using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace CriThink.Client.Core.ViewModels.Users
{
    public class WelcomeLoginSignInViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public WelcomeLoginSignInViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        }

        #region Commands

        private IMvxAsyncCommand _navigateToSignInViewCommand;
        public IMvxAsyncCommand NavigateToSignInViewCommand => _navigateToSignInViewCommand ??= new MvxAsyncCommand(DoNavigateToSignInViewCommand);


        private IMvxAsyncCommand _navigateToLoginViewCommand;
        public IMvxAsyncCommand NavigateToLoginViewCommand => _navigateToLoginViewCommand ??= new MvxAsyncCommand(DoNavigateToLoginViewCommand);

        #endregion

        private async Task DoNavigateToSignInViewCommand()
        {
            await _navigationService.Navigate<SignUpViewModel>().ConfigureAwait(true);
        }

        private async Task DoNavigateToLoginViewCommand()
        {
            await _navigationService.Navigate<LoginViewModel>().ConfigureAwait(true);
        }
    }
}
