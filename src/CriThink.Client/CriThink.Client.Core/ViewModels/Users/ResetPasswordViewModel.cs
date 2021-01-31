using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using CriThink.Client.Core.Constants;
using CriThink.Client.Core.Services;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace CriThink.Client.Core.ViewModels.Users
{
    public class ResetPasswordViewModel : BaseViewModel
    {
        private readonly IIdentityService _identityService;
        private readonly IMvxNavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IMvxLog _log;

        public ResetPasswordViewModel(IIdentityService identityService, IMvxNavigationService navigationService, IUserDialogs userDialogs, IMvxLogProvider logProvider)
        {
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _userDialogs = userDialogs ?? throw new ArgumentNullException(nameof(userDialogs));
            _log = logProvider?.GetLogFor<ResetPasswordViewModel>();
        }

        protected override void InitFromBundle(IMvxBundle parameters)
        {
            base.InitFromBundle(parameters);

            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            if (parameters.Data.ContainsKey("code"))
                Token = parameters.Data["code"];

            if (parameters.Data.ContainsKey("userId"))
                UserId = parameters.Data["userId"];
        }

        #region Properties

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private string _repeatPassword;
        public string RepeatPassword
        {
            get => _repeatPassword;
            set => SetProperty(ref _repeatPassword, value);
        }

        public string Token { get; private set; }

        public string UserId { get; private set; }

        #endregion

        #region Commands

        private IMvxAsyncCommand _sendRequestCommand;

        public IMvxAsyncCommand SendRequestCommand => _sendRequestCommand ??= new MvxAsyncCommand(DoSendRequestCommand);

        #endregion

        private async Task DoSendRequestCommand(CancellationToken cancellationToken)
        {
            IsLoading = true;

            var request = new ResetPasswordRequest
            {
                NewPassword = Password,
                Token = Token,
                UserId = UserId
            };

            var response = await _identityService.ResetPasswordAsync(request, cancellationToken).ConfigureAwait(false);
            if (response is null)
            {
                await ShowErrorMessage().ConfigureAwait(true);
                return;
            }

            await ShowWelcomeMessage(response).ConfigureAwait(true);

            try
            {
                await _navigationService.Navigate<HomeViewModel>(cancellationToken: cancellationToken, presentationBundle: new MvxBundle(new Dictionary<string, string>
                {
                    {MvxBundleConstaints.ClearBackStack, ""}
                })).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _log?.FatalException("Can't navigate to home after password reset", ex);
            }
            finally
            {
                IsLoading = false;
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
