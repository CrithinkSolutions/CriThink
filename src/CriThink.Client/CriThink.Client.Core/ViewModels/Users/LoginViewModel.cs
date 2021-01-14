using System;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using CriThink.Client.Core.Services;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using CriThink.Common.Helpers;
using MvvmCross.Commands;
using MvvmCross.Navigation;

namespace CriThink.Client.Core.ViewModels.Users
{
    public class LoginViewModel : BaseSocialLoginViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;

        public LoginViewModel(IMvxNavigationService navigationService, IIdentityService identityService, IUserDialogs userDialogs)
            : base(identityService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _userDialogs = userDialogs ?? throw new ArgumentNullException(nameof(userDialogs));
        }

        #region Properties

        private string _emailOrUsername;
        public string EmailOrUsername
        {
            get => _emailOrUsername;
            set
            {
                SetProperty(ref _emailOrUsername, value);
                RaisePropertyChanged(() => LoginCommand);
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password, value);
                RaisePropertyChanged(() => LoginCommand);
            }
        }

        #endregion

        #region Commands

        private IMvxAsyncCommand _loginCommand;
        public IMvxAsyncCommand LoginCommand => _loginCommand ??= new MvxAsyncCommand(DoLoginCommand, () => !string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(EmailOrUsername));

        private IMvxAsyncCommand _navigateToHomeCommand;
        public IMvxAsyncCommand NavigateToHomeCommand => _navigateToHomeCommand ??= new MvxAsyncCommand(DoNavigateToHomeCommand);

        private IMvxAsyncCommand _navigateToForgotPasswordCommand;
        public IMvxAsyncCommand NavigateToForgotPasswordCommand => _navigateToForgotPasswordCommand ??= new MvxAsyncCommand(DoNavigateToForgotPasswordCommand);

        #endregion

        private async Task DoLoginCommand(CancellationToken cancellationToken)
        {
            var request = new UserLoginRequest
            {
                Password = Password
            };

            var isEmail = EmailHelper.IsEmail(EmailOrUsername);
            if (isEmail)
                request.Email = EmailOrUsername.ToUpperInvariant();
            else
                request.UserName = EmailOrUsername.ToUpperInvariant();

            var userInfo = await IdentityService.PerformLoginAsync(request, cancellationToken).ConfigureAwait(false);
            if (userInfo == null)
            {
                await ShowErrorMessage(cancellationToken).ConfigureAwait(false);
            }
            else
            {
                await _navigationService.Navigate<HomeViewModel>(cancellationToken: cancellationToken).ConfigureAwait(true);
            }
        }

        private async Task DoNavigateToHomeCommand()
        {
            await _navigationService.Navigate<HomeViewModel>().ConfigureAwait(true);
        }

        private async Task DoNavigateToForgotPasswordCommand()
        {
            await _navigationService.Navigate<ForgotPasswordViewModel>().ConfigureAwait(true);
        }

        private async Task ShowErrorMessage(CancellationToken cancellationToken)
        {
            await _userDialogs.AlertAsync(
                message: "Incorrect email address or password. PLease check and try again",
                okText: "Ok",
                cancelToken: cancellationToken).ConfigureAwait(true);
        }
    }
}
