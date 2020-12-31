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
    public class SignUpEmailViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IIdentityService _identityService;

        public SignUpEmailViewModel(IMvxNavigationService navigationService, IIdentityService identityService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }

        #region Properties

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _username;
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

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

        #endregion

        private IMvxAsyncCommand _signUpCommand;
        public IMvxAsyncCommand SignUpCommand => _signUpCommand ??= new MvxAsyncCommand(DoSignUpCommand);

        private async Task DoSignUpCommand(CancellationToken cancellationToken)
        {
            if (Password != RepeatPassword)
                return;

            var request = new UserSignUpRequest
            {
                Password = Password,
                Email = Email,
                UserName = Username
            };

            await _identityService.PerformSignUpAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}
