using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using CriThink.Client.Core.Constants;
using CriThink.Client.Core.Services;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace CriThink.Client.Core.ViewModels.Users
{
    public class ResetPasswordViewModel : BaseViewModel
    {
        private readonly IIdentityService _identityService;
        private readonly IMvxNavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly ILogger<ResetPasswordViewModel> _logger;

        private string _token;
        private string _userId;

        public ResetPasswordViewModel(IIdentityService identityService, IMvxNavigationService navigationService, IUserDialogs userDialogs, ILogger<ResetPasswordViewModel> logger)
        {
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _userDialogs = userDialogs ?? throw new ArgumentNullException(nameof(userDialogs));
            _logger = logger;
        }

        protected override void InitFromBundle(IMvxBundle parameters)
        {
            base.InitFromBundle(parameters);

            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            if (parameters.Data.ContainsKey("code"))
                _token = parameters.Data["code"];

            if (parameters.Data.ContainsKey("userId"))
                _userId = parameters.Data["userId"];
        }

        #region Properties

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password, value);
                RaisePropertyChanged(() => SendRequestCommand);
            }
        }

        private string _repeatPassword;
        public string RepeatPassword
        {
            get => _repeatPassword;
            set
            {
                SetProperty(ref _repeatPassword, value);
                RaisePropertyChanged(() => SendRequestCommand);
            }
        }

        #endregion

        #region Commands

        private IMvxAsyncCommand _sendRequestCommand;

        public IMvxAsyncCommand SendRequestCommand => _sendRequestCommand ??= new MvxAsyncCommand(DoSendRequestCommand,
            () =>
                !IsLoading &&
                !string.IsNullOrWhiteSpace(Password) &&
                string.Equals(Password, RepeatPassword, StringComparison.CurrentCulture) &&
                !string.IsNullOrWhiteSpace(_userId));

        #endregion

        public override void Prepare()
        {
            base.Prepare();
            _logger?.LogInformation("User navigates to reset password");
        }

        private async Task DoSendRequestCommand(CancellationToken cancellationToken)
        {
            IsLoading = true;

            var request = new ResetPasswordRequest
            {
                NewPassword = Password,
                Token = _token,
                UserId = _userId
            };

            try
            {
                var response = await _identityService.ResetPasswordAsync(request, cancellationToken).ConfigureAwait(false);
                if (response is null)
                {
                    await ShowErrorMessage().ConfigureAwait(true);
                    return;
                }

                await ShowWelcomeMessage(response).ConfigureAwait(true);


                await _navigationService.Navigate<HomeViewModel>(cancellationToken: cancellationToken, presentationBundle: new MvxBundle(new Dictionary<string, string>
                {
                    {MvxBundleConstaints.ClearBackStack, ""}
                })).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger?.LogCritical(ex, "Can't navigate to home after password reset");
            }
            finally
            {
                IsLoading = false;
                await RaisePropertyChanged(() => SendRequestCommand);
            }
        }

        private Task ShowWelcomeMessage(VerifyUserEmailResponse response)
        {
            var localiedText = LocalizedTextSource.GetText("ConfirmPasswordReset");
            return _userDialogs.AlertAsync(string.Format(CultureInfo.CurrentCulture, localiedText, response.Username));
        }

        private Task ShowErrorMessage()
        {
            var localizedText = LocalizedTextSource.GetText("ErrorPasswordReset");
            return _userDialogs.AlertAsync(localizedText);
        }
    }
}
