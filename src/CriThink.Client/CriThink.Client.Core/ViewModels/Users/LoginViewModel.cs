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
            : base(identityService, userDialogs, navigationService, logProvider)
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
        public IMvxAsyncCommand LoginCommand => _loginCommand ??= new MvxAsyncCommand(DoLoginCommand,
            () =>
            !IsLoading &&
            !string.IsNullOrWhiteSpace(Password) &&
            !string.IsNullOrWhiteSpace(EmailOrUsername) && !IsLoading);

        private IMvxAsyncCommand _navigateToForgotPasswordCommand;
        public IMvxAsyncCommand NavigateToForgotPasswordCommand => _navigateToForgotPasswordCommand ??= new MvxAsyncCommand(DoNavigateToForgotPasswordCommand,
            () =>
            !IsLoading);

        #endregion

        public override void Prepare()
        {
            base.Prepare();
            Log?.Info("User navigates to login");
        }

        private async Task DoLoginCommand(CancellationToken cancellationToken)
        {
            IsLoading = true;

            UserLoginResponse userInfo;
            try
            {
                var request = new UserLoginRequest { Password = Password };

                if (EmailHelper.IsEmail(EmailOrUsername))
                    request.Email = EmailOrUsername.ToUpperInvariant();
                else
                    request.UserName = EmailOrUsername.ToUpperInvariant();

                userInfo = await IdentityService.PerformLoginAsync(request, cancellationToken)
                    .ConfigureAwait(false);
            }
            finally
            {
                IsLoading = false;
                await RaisePropertyChanged(() => LoginCommand);
            }

            if (userInfo is null)
            {
                var localizedText = LocalizedTextSource.GetText("LoginErrorMessage");
                await ShowErrorMessageAsync(localizedText).ConfigureAwait(false);
            }
            else
            {
                await _navigationService.Navigate<HomeViewModel>(
                    new MvxBundle(new Dictionary<string, string>
                    {
                        {MvxBundleConstaints.ClearBackStack, ""}
                    }),
                    cancellationToken: cancellationToken).ConfigureAwait(true);
            }
        }

        private async Task DoNavigateToForgotPasswordCommand()
        {
            await _navigationService.Navigate<ForgotPasswordViewModel>().ConfigureAwait(true);
        }
    }
}
