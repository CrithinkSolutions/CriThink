using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using CriThink.Client.Core.Services;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using CriThink.Common.Helpers;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace CriThink.Client.Core.ViewModels.Users
{
    public class SignUpEmailViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IIdentityService _identityService;
        private readonly IUserDialogs _userDialogs;
        private readonly IMvxLog _log;

        public SignUpEmailViewModel(IMvxNavigationService navigationService, IIdentityService identityService, IUserDialogs userDialogs, IMvxLogProvider logProvider)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _userDialogs = userDialogs ?? throw new ArgumentNullException(nameof(userDialogs));
            _log = logProvider?.GetLogFor<SignUpEmailViewModel>();
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
            !IsLoading &&
            !string.IsNullOrWhiteSpace(Email) && EmailHelper.IsEmail(Email) &&
            !string.IsNullOrWhiteSpace(Username) &&
            !string.IsNullOrWhiteSpace(Password) &&
            string.Equals(Password, RepeatPassword, StringComparison.CurrentCulture));

        public override void Prepare()
        {
            base.Prepare();
            _log?.Info("User navigates to sign up with email");
        }

        private async Task DoSignUpCommand(CancellationToken cancellationToken)
        {
            IsLoading = true;

            var request = new UserSignUpRequest
            {
                Password = Password,
                Email = Email,
                UserName = Username
            };

            try
            {
                var userInfo = await _identityService.PerformSignUpAsync(request, cancellationToken)
                    .ConfigureAwait(false);
                if (userInfo is null)
                {
                    var localizedText = LocalizedTextSource.GetText("ErrorMessage");
                    await ShowMessage(localizedText).ConfigureAwait(true);
                }
                else
                {
                    var localizedText = LocalizedTextSource.GetText("WelcomeMessage");
                    await ShowMessage(localizedText).ConfigureAwait(true);

                    await _navigationService.Navigate<LoginViewModel>(cancellationToken: cancellationToken)
                        .ConfigureAwait(true);
                }
            }
            catch (Exception ex)
            {
                _log?.ErrorException("Error during the user sign up", ex);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task ConfirmUserEmailAsync(string userId, string code)
        {
            IsLoading = true;

            try
            {
                var userInfo = await _identityService.ConfirmUserEmailAsync(userId, code).ConfigureAwait(false);
                if (userInfo == null)
                {
                    var localizedText = LocalizedTextSource.GetText("ConfirmErrorMessage");
                    await ShowMessage(localizedText).ConfigureAwait(true);
                }
                else
                {
                    var localizedText = LocalizedTextSource.GetText("ConfirmWelcomeMessage");
                    await ShowMessage(string.Format(CultureInfo.CurrentCulture, localizedText, userInfo.Username)).ConfigureAwait(true);
                    await _navigationService.Navigate<LoginViewModel>().ConfigureAwait(true);
                }
            }
            catch (Exception ex)
            {
                _log?.ErrorException("Error confirming user email", ex);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private Task ShowMessage(string message)
        {
            return _userDialogs.AlertAsync(
                message,
                okText: "Ok");
        }
    }
}
