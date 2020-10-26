using System;
using System.Threading.Tasks;
using CriThink.Client.Core.Services;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace CriThink.Client.Core.ViewModels.Users
{
    public class LoginViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IIdentityService _identityService;

        public LoginViewModel(IMvxNavigationService navigationService, IIdentityService identityService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }

        #region Commands

        private IMvxAsyncCommand _loginCommand;
        public IMvxAsyncCommand LoginCommand => _loginCommand ??= new MvxAsyncCommand(DoLoginCommand);

        private IMvxAsyncCommand _forgotPasswordCommand;
        public IMvxAsyncCommand ForgotPasswordCommand => _forgotPasswordCommand ??= new MvxAsyncCommand(DoForgotPasswordCommand);

        private IMvxAsyncCommand _navigateToHomeCommand;
        public IMvxAsyncCommand NavigateToHomeCommand => _navigateToHomeCommand ??= new MvxAsyncCommand(DoNavigateToHomeCommand);

        #endregion

        private Task DoForgotPasswordCommand()
        {
            return Task.CompletedTask;
        }

        private async Task DoLoginCommand()
        {
            await _identityService.PerformLoginAsync(new UserLoginRequest()).ConfigureAwait(false);
        }

        private async Task DoNavigateToHomeCommand()
        {
            await _navigationService.Navigate<HomeViewModel>().ConfigureAwait(true);
        }
    }
}
