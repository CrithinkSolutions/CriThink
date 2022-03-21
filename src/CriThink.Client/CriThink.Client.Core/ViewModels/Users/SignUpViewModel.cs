﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using CriThink.Client.Core.Services;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using CriThink.Common.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;

namespace CriThink.Client.Core.ViewModels.Users
{
    public class SignUpViewModel : BaseSocialLoginViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public SignUpViewModel(
            IMvxNavigationService navigationService,
            IIdentityService identityService,
            IConfiguration configuration,
            IUserDialogs userDialogs,
            ILogger<BaseSocialLoginViewModel> logger)
            : base(
                  identityService,
                  userDialogs,
                  configuration,
                  navigationService,
                  logger)
        {
            _navigationService = navigationService ??
                throw new ArgumentNullException(nameof(navigationService));
        }

        #region Commands

        private IMvxAsyncCommand _navigateToSignUpEmailCommand;
        public IMvxCommand NavigateToSignUpEmailCommand => _navigateToSignUpEmailCommand ??= new MvxAsyncCommand(DoNavigateToSignUpEmailCommand);

        private IMvxAsyncCommand _navigateToLoginCommand;
        public IMvxCommand NavigateToLoginCommand => _navigateToLoginCommand ??= new MvxAsyncCommand(DoNavigateToLoginCommand);

        private IMvxAsyncCommand<string> _restoreAccountCommand;
        public IMvxAsyncCommand<string> RestoreAccountCommand => _restoreAccountCommand ??= new MvxAsyncCommand<string>(DoRestoreAccountCommand);

        #endregion

        public override void Prepare()
        {
            base.Prepare();
            Logger?.LogInformation("User navigates to sign up");
        }

        private async Task DoNavigateToLoginCommand()
        {
            await _navigationService.Navigate<LoginViewModel>().ConfigureAwait(true);
        }

        private async Task DoNavigateToSignUpEmailCommand()
        {
            await _navigationService.Navigate<SignUpEmailViewModel>().ConfigureAwait(false);
        }

        private async Task DoRestoreAccountCommand(string emailOrUsername, CancellationToken cancellationToken)
        {
            IsLoading = true;

            var message = string.Empty;
            var title = string.Empty;
            var request = new RestoreUserRequest();

            var isEmail = EmailHelper.IsEmail(emailOrUsername);
            if (isEmail)
                request.Email = emailOrUsername;
            else
                request.Username = emailOrUsername;

            try
            {
                await IdentityService.RestoreDeletedAccountAsync(request, cancellationToken).ConfigureAwait(true);

                message = LocalizedTextSource.GetText("RestoreAccountConfirmationMessage");
                title = LocalizedTextSource.GetText("RestoreAccountConfirmationTitle");
            }
            catch (Exception)
            {
                message = LocalizedTextSource.GetText("RestoreAccountErrorMessage");
                title = LocalizedTextSource.GetText("RestoreAccountErrorTitle");
            }
            finally
            {
                IsLoading = false;
                await UserDialogs.AlertAsync(message, title, cancelToken: cancellationToken);
            }
        }
    }
}
