using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace CriThink.Client.Core.ViewModels.Users
{
    public class LoginViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public LoginViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        }

        private IMvxAsyncCommand _loginCommand;
        public IMvxAsyncCommand LoginCommand => _loginCommand ??= new MvxAsyncCommand(DoLoginCommand);

        private IMvxAsyncCommand _forgotPasswordCommand;
        public IMvxAsyncCommand ForgotPasswordCommand => _forgotPasswordCommand ??= new MvxAsyncCommand(DoForgotPasswordCommand);

        private IMvxAsyncCommand _navigateToHomeCommand;
        public IMvxAsyncCommand NavigateToHomeCommand => _navigateToHomeCommand ??= new MvxAsyncCommand(DoNavigateToHomeCommand);

        private Task DoForgotPasswordCommand()
        {
            return Task.CompletedTask;
        }

        private Task DoLoginCommand()
        {
            return Task.CompletedTask;
        }

        private async Task DoNavigateToHomeCommand()
        {
            await _navigationService.Navigate<HomeViewModel>().ConfigureAwait(true);
        }
    }
}
