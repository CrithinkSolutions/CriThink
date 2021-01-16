﻿using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using CriThink.Client.Core.Services;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using CriThink.Common.Helpers;
using Microsoft.Extensions.Logging;
using MvvmCross.ViewModels;

namespace CriThink.Client.Core.ViewModels.Users
{
    public class BaseSocialLoginViewModel : MvxViewModel
    {
        private readonly IUserDialogs _userDialogs;
        private readonly ILogger<BaseSocialLoginViewModel> _logger;

        public BaseSocialLoginViewModel(IIdentityService identityService, IUserDialogs userDialogs, ILogger<BaseSocialLoginViewModel> logger)
        {
            IdentityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _userDialogs = userDialogs ?? throw new ArgumentNullException(nameof(userDialogs));
            _logger = logger;
        }

        protected IIdentityService IdentityService { get; }

        public async Task PerformLoginSignInAsync(string token, ExternalLoginProvider loginProvider)
        {
            try
            {
                var request = new ExternalLoginProviderRequest
                {
                    SocialProvider = loginProvider,
                    UserToken = Base64Helper.ToBase64(token),
                };

                await IdentityService.PerformSocialLoginSignInAsync(request).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error while loggin using social login", string.IsNullOrWhiteSpace(token), loginProvider);
                await ShowErrorMessage($"An error occurred when logging in with {loginProvider}").ConfigureAwait(true);
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
