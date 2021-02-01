using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using CriThink.Client.Core.Constants;
using CriThink.Client.Core.Services;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using CriThink.Common.Helpers;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace CriThink.Client.Core.ViewModels.Users
{
    public class LoginViewModel : BaseSocialLoginViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public LoginViewModel(IMvxNavigationService navigationService, IIdentityService identityService, IUserDialogs userDialogs, IMvxLogProvider logProvider)
            : base(identityService, userDialogs, logProvider)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
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

        public override void Prepare()
        {
            base.Prepare();
            Log?.Info("User navigates to login");
        }

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
            if (userInfo is null)
            {
                await ShowErrorMessage("Incorrect email address or password. Please check and try again").ConfigureAwait(false);
            }
            else
            {
                await _navigationService.Navigate<HomeViewModel>(cancellationToken: cancellationToken).ConfigureAwait(true);
            }
        }

        private async Task DoNavigateToHomeCommand()
        {
            try
            {
                await _navigationService.Navigate<HomeViewModel>(new MvxBundle(new Dictionary<string, string>
                {
                    {MvxBundleConstaints.ClearBackStack, ""}
                })).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log?.FatalException("Error clearing back stack", ex);
            }
        }

        private async Task DoNavigateToForgotPasswordCommand()
        {
            await _navigationService.Navigate<ForgotPasswordViewModel>().ConfigureAwait(true);
        }
    }
}
