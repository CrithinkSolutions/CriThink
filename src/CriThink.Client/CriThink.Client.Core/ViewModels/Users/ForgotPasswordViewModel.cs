﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using CriThink.Client.Core.Services;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using CriThink.Common.Helpers;
using MvvmCross.Commands;

namespace CriThink.Client.Core.ViewModels.Users
{
    public class ForgotPasswordViewModel : BaseViewModel
    {
        private readonly IIdentityService _identityService;
        private readonly IUserDialogs _userDialogs;

        public ForgotPasswordViewModel(IIdentityService identityService, IUserDialogs userDialogs)
        {
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _userDialogs = userDialogs ?? throw new ArgumentNullException(nameof(userDialogs));
        }

        private string _emailOrUsername;

        public string EmailOrUsername
        {
            get => _emailOrUsername;
            set => SetProperty(ref _emailOrUsername, value);
        }

        private IMvxAsyncCommand _sendRequestCommand;

        public IMvxAsyncCommand SendRequestCommand => _sendRequestCommand ??= _sendRequestCommand = new MvxAsyncCommand(DoSendRequestCommand);

        private async Task DoSendRequestCommand(CancellationToken cancellationToken)
        {
            var request = new ForgotPasswordRequest();

            var isEmail = EmailHelper.IsEmail(EmailOrUsername);
            if (isEmail)
                request.Email = EmailOrUsername.ToUpperInvariant();
            else
                request.UserName = EmailOrUsername.ToUpperInvariant();

            try
            {
                await _identityService.RequestTemporaryTokenAsync(request, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception)
            {
                await ShowErrorMessage("An error occurred requesting a temporary token").ConfigureAwait(true);
            }
        }

        public async Task ShowErrorMessage(string message)
        {
            await _userDialogs.AlertAsync(
                message,
                okText: "Ok").ConfigureAwait(true);
        }
    }
}
