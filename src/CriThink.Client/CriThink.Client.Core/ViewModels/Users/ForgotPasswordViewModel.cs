using System;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using CriThink.Client.Core.Services;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using CriThink.Common.Helpers;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;

namespace CriThink.Client.Core.ViewModels.Users
{
    public class ForgotPasswordViewModel : BaseViewModel
    {
        private readonly IIdentityService _identityService;
        private readonly IUserDialogs _userDialogs;
        private readonly IMvxNavigationService _navigationService;
        private readonly ILogger<ForgotPasswordViewModel> _logger;

        public ForgotPasswordViewModel(IIdentityService identityService, IUserDialogs userDialogs, ILogger<ForgotPasswordViewModel> logger, IMvxNavigationService navigationService)
        {
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _userDialogs = userDialogs ?? throw new ArgumentNullException(nameof(userDialogs));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _logger = logger;
        }

        private string _emailOrUsername;
        public string EmailOrUsername
        {
            get => _emailOrUsername;
            set
            {
                SetProperty(ref _emailOrUsername, value);
                RaisePropertyChanged(() => SendRequestCommand);
            }
        }

        private IMvxAsyncCommand _sendRequestCommand;

        public IMvxAsyncCommand SendRequestCommand => _sendRequestCommand ??= _sendRequestCommand = new MvxAsyncCommand(DoSendRequestCommand,
            () =>
                !IsLoading &&
                !string.IsNullOrWhiteSpace(EmailOrUsername));

        public override void Prepare()
        {
            base.Prepare();
            _logger?.LogInformation("User navigates to forgot password");
        }

        private async Task DoSendRequestCommand(CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(EmailOrUsername))
                return;

            IsLoading = true;

            var request = new ForgotPasswordRequest();

            var isEmail = EmailHelper.IsEmail(EmailOrUsername);
            if (isEmail)
                request.Email = EmailOrUsername.ToUpperInvariant();
            else
                request.UserName = EmailOrUsername.ToUpperInvariant();

            string localizedSuccessfulMessage;
            string localizedSuccessfulOk;

            try
            {
                await _identityService.RequestTemporaryTokenAsync(request, cancellationToken).ConfigureAwait(false);

                localizedSuccessfulMessage = LocalizedTextSource.GetText("RequestMessage");
                localizedSuccessfulOk = LocalizedTextSource.GetText("RequestOk");

                await ShowMessage(localizedSuccessfulMessage, localizedSuccessfulOk).ConfigureAwait(true);

                await _navigationService.Close(this, cancellationToken).ConfigureAwait(true);
            }
            catch (Exception)
            {
                localizedSuccessfulMessage = LocalizedTextSource.GetText("RequestErrorMessage");
                localizedSuccessfulOk = LocalizedTextSource.GetText("RequestErrorOk");

                await ShowMessage(localizedSuccessfulMessage, localizedSuccessfulOk).ConfigureAwait(true);
            }
            finally
            {
                IsLoading = false;
                await RaisePropertyChanged(() => SendRequestCommand);
            }
        }

        private async Task ShowMessage(string message, string okText)
        {
            await _userDialogs.AlertAsync(
                message,
                okText: okText).ConfigureAwait(true);
        }
    }
}
