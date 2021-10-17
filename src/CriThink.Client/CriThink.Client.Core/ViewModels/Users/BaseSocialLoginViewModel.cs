using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Acr.UserDialogs;
using CriThink.Client.Core.Constants;
using CriThink.Client.Core.Services;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using CriThink.Common.Helpers;
using Microsoft.Extensions.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace CriThink.Client.Core.ViewModels.Users
{
    public class BaseSocialLoginViewModel : BaseViewModel
    {
        protected readonly IUserDialogs UserDialogs;
        private readonly IMvxNavigationService _navigationService;

        public BaseSocialLoginViewModel(IIdentityService identityService, IUserDialogs userDialogs, IMvxNavigationService navigationService, ILogger<BaseSocialLoginViewModel> logger)
        {
            IdentityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            UserDialogs = userDialogs ?? throw new ArgumentNullException(nameof(userDialogs));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            Logger = logger;
        }

        protected IIdentityService IdentityService { get; }

        protected ILogger<BaseSocialLoginViewModel> Logger { get; }

        public async Task PerformLoginSignInAsync(string token, ExternalLoginProvider loginProvider)
        {
            IsLoading = true;

            try
            {
                var request = new ExternalLoginProviderRequest
                {
                    SocialProvider = loginProvider,
                    UserToken = Base64Helper.ToBase64(token),
                };

                await IdentityService.PerformSocialLoginSignInAsync(request).ConfigureAwait(false);

                await _navigationService.Navigate<HomeViewModel>(
                    new MvxBundle(new Dictionary<string, string>
                    {
                        {MvxBundleConstaints.ClearBackStack, ""}
                    })).ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                Logger?.LogCritical(ex, "Error while loggin using social login", string.IsNullOrWhiteSpace(token), loginProvider);

                var localizedErrorText = LocalizedTextSource.GetText("SocialLoginErrorMessage");

                await ShowErrorMessageAsync(
                    ex,
                    string.Format(
                        CultureInfo.CurrentUICulture,
                        localizedErrorText,
                        loginProvider))
                    .ConfigureAwait(true);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public Task ShowErrorMessageAsync(Exception ex, string message)
        {
            Logger?.LogCritical(ex, message);
            return ShowErrorMessageAsync(message);
        }

        public async Task ShowErrorMessageAsync(string message)
        {
            await UserDialogs.AlertAsync(
                message,
                okText: "Ok").ConfigureAwait(true);
        }
    }
}
