using System;
using System.Threading.Tasks;
using CriThink.Client.Core.ViewModels.Users;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace CriThink.Client.Core.ViewModels
{
    public class HomeViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public HomeViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        }

        #region Properties

        private IMvxAsyncCommand _navigateLoginViewCommand;
        // ReSharper disable once UnusedMember.Global
        public IMvxAsyncCommand NavigateLoginViewCommand => _navigateLoginViewCommand ??= new MvxAsyncCommand(DoNavigateLoginViewCommand);

        private IMvxAsyncCommand _navigateSignUpViewCommand;
        // ReSharper disable once UnusedMember.Global
        public IMvxAsyncCommand NavigateSignUpViewCommand => _navigateSignUpViewCommand ??= new MvxAsyncCommand(DoNavigateSignUpViewCommand);

        #endregion

        private async Task DoNavigateLoginViewCommand()
        {
            await _navigationService.Navigate<LoginViewModel>().ConfigureAwait(true);
        }

        private async Task DoNavigateSignUpViewCommand()
        {
            await _navigationService.Navigate<SignUpViewModel>().ConfigureAwait(true);
        }
    }
}
