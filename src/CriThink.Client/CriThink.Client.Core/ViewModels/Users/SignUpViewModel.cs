using System;
using System.Threading.Tasks;
using CriThink.Client.Core.Services;
using MvvmCross.Commands;
using MvvmCross.Navigation;

namespace CriThink.Client.Core.ViewModels.Users
{
    public class SignUpViewModel : BaseSocialLoginViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public SignUpViewModel(IMvxNavigationService navigationService, IIdentityService identityService)
            : base(identityService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        }

        #region Commands

        private IMvxAsyncCommand _navigateToSignUpEmailCommand;
        public IMvxCommand NavigateToSignUpEmailCommand => _navigateToSignUpEmailCommand ??= new MvxAsyncCommand(DoNavigateToSignUpEmailCommand);

        private IMvxAsyncCommand _navigateToLoginCommand;
        public IMvxCommand NavigateToLoginCommand => _navigateToLoginCommand ??= new MvxAsyncCommand(DoNavigateToLoginCommand);

        #endregion

        private async Task DoNavigateToLoginCommand()
        {
            await _navigationService.Navigate<LoginViewModel>().ConfigureAwait(true);
        }

        private async Task DoNavigateToSignUpEmailCommand()
        {
            await _navigationService.Navigate<SignUpEmailViewModel>().ConfigureAwait(false);
        }
    }
}
