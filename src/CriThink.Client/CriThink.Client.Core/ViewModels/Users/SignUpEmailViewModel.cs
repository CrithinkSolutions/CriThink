using System;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using CriThink.Client.Core.Services;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using CriThink.Common.Helpers;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace CriThink.Client.Core.ViewModels.Users
{
    public class SignUpEmailViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IIdentityService _identityService;
        private readonly IUserDialogs _userDialogs;

        public SignUpEmailViewModel(IMvxNavigationService navigationService, IIdentityService identityService, IUserDialogs userDialogs)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _userDialogs = userDialogs ?? throw new ArgumentNullException(nameof(userDialogs));
        }

        #region Properties

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                SetProperty(ref _email, value);
                RaisePropertyChanged(() => SignUpCommand);
            }
        }

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                SetProperty(ref _username, value);
                RaisePropertyChanged(() => SignUpCommand);
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password, value);
                RaisePropertyChanged(() => SignUpCommand);
            }
        }

        private string _repeatPassword;
        public string RepeatPassword
        {
            get => _repeatPassword;
            set
            {
                SetProperty(ref _repeatPassword, value);
                RaisePropertyChanged(() => SignUpCommand);
            }
        }

        #endregion

        private IMvxAsyncCommand _signUpCommand;
        public IMvxAsyncCommand SignUpCommand => _signUpCommand ??= new MvxAsyncCommand(DoSignUpCommand, () =>
            EmailHelper.IsEmail(Email) &&
            !string.IsNullOrWhiteSpace(Username) &&
            !string.IsNullOrWhiteSpace(Password) &&
            string.Equals(Password, RepeatPassword, StringComparison.CurrentCulture));

        private async Task DoSignUpCommand(CancellationToken cancellationToken)
        {
            var request = new UserSignUpRequest
            {
                Password = Password,
                Email = Email,
                UserName = Username
            };

            var userInfo = await _identityService.PerformSignUpAsync(request, cancellationToken).ConfigureAwait(false);
            if (userInfo == null)
            {
                await ShowErrorMessage("Error occurred while sign in.").ConfigureAwait(false);
            }
            else
            {
                await ShowErrorMessage("Thank you for joining us! We sent you an email with a confirmation link. Please click on that link and perform the login").ConfigureAwait(true);
                await _navigationService.Navigate<LoginViewModel>(cancellationToken: cancellationToken).ConfigureAwait(true);
            }
        }

        public async Task ShowErrorMessage(string message)
        {
            await _userDialogs.AlertAsync(
                message,
                okText: "Ok").ConfigureAwait(true);
        }

        public async Task ConfirmUserEmailAsync(string userId, string code)
        {
            var userInfo = await _identityService.ConfirmUserEmailAsync(userId, code).ConfigureAwait(false);
            if (userInfo == null)
            {
                await ShowErrorMessage("An error occurred while confirming your email").ConfigureAwait(true);
            }
            else
            {
                await ShowErrorMessage($"Hello {userInfo.Username}! You now can login").ConfigureAwait(true);
                await _navigationService.Navigate<LoginViewModel>().ConfigureAwait(true);
            }
        }
    }
}
