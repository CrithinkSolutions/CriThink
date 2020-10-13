using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace CriThink.Client.Core.ViewModels.Users
{
    public class SignUpViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public SignUpViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        }

        private IMvxAsyncCommand _navigateToSignUpEmailCommand;
        public IMvxCommand NavigateToSignUpEmailCommand => _navigateToSignUpEmailCommand ??= new MvxAsyncCommand(DoNavigateToSignUpEmailCommand);

        private IMvxAsyncCommand _navigateToLoginCommand;
        public IMvxCommand NavigateToLoginCommand => _navigateToLoginCommand ??= new MvxAsyncCommand(DoNavigateToLoginCommand);

        private async Task DoNavigateToLoginCommand()
        {
            await _navigationService.Navigate<LoginViewModel>().ConfigureAwait(true);
        }

        private Task DoNavigateToSignUpEmailCommand()
        {
            throw new System.NotImplementedException();
        }
    }
}
