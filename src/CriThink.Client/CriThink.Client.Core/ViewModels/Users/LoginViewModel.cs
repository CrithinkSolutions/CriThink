using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Services;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace CriThink.Client.Core.ViewModels.Users
{
    public class LoginViewModel : MvxViewModel, IDisposable
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IIdentityService _identityService;
        private readonly CancellationTokenSource _cancellationToken;

        public LoginViewModel(IMvxNavigationService navigationService, IIdentityService identityService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _cancellationToken = new CancellationTokenSource();
        }

        #region Properties

        private string _emailOrUsername;

        public string EmailOrUsername
        {
            get => _emailOrUsername;
            set => SetProperty(ref _emailOrUsername, value);
        }

        private string _password;

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        #endregion

        #region Commands

        private IMvxAsyncCommand _loginCommand;
        public IMvxAsyncCommand LoginCommand => _loginCommand ??= new MvxAsyncCommand(DoLoginCommand);

        private IMvxAsyncCommand _forgotPasswordCommand;
        public IMvxAsyncCommand ForgotPasswordCommand => _forgotPasswordCommand ??= new MvxAsyncCommand(DoForgotPasswordCommand);

        private IMvxAsyncCommand _navigateToHomeCommand;
        public IMvxAsyncCommand NavigateToHomeCommand => _navigateToHomeCommand ??= new MvxAsyncCommand(DoNavigateToHomeCommand);

        #endregion

        private Task DoForgotPasswordCommand()
        {
            return Task.CompletedTask;
        }

        private async Task DoLoginCommand()
        {
            var request = new UserLoginRequest
            {
                Password = Password
            };

            var isEmail = Regex.IsMatch(EmailOrUsername, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);

            if (isEmail)
                request.Email = EmailOrUsername.ToUpperInvariant();
            else
                request.UserName = EmailOrUsername.ToUpperInvariant();

            await _identityService.PerformLoginAsync(request, _cancellationToken.Token).ConfigureAwait(false);
        }

        private async Task DoNavigateToHomeCommand()
        {
            await _navigationService.Navigate<HomeViewModel>().ConfigureAwait(true);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _cancellationToken?.Dispose();
            }
        }
    }
}
