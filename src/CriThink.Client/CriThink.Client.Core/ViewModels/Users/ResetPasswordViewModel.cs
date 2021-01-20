using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Services;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace CriThink.Client.Core.ViewModels.Users
{
    public class ResetPasswordViewModel : BaseViewModel
    {
        private readonly IIdentityService _identityService;
        private readonly IMvxNavigationService _navigationService;

        public ResetPasswordViewModel(IIdentityService identityService, IMvxNavigationService navigationService)
        {
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
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

            var _ = await _identityService.ResetPasswordAsync(request, cancellationToken).ConfigureAwait(false);

            //TODO: show welcome message
            await _navigationService.Navigate<HomeViewModel>(cancellationToken: cancellationToken).ConfigureAwait(true);

            IsLoading = false;
        }
    }
}
