using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Acr.UserDialogs;
using CriThink.Client.Core.Constants;
using CriThink.Client.Core.Services;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace CriThink.Client.Core.ViewModels.Users
{
    public class BaseSocialLoginViewModel : BaseViewModel
    {
        protected readonly IUserDialogs UserDialogs;
        private readonly IConfiguration _configuration;
        private readonly IMvxNavigationService _navigationService;

        public BaseSocialLoginViewModel(
            IIdentityService identityService,
            IUserDialogs userDialogs,
            IConfiguration configuration,
            IMvxNavigationService navigationService,
            ILogger<BaseSocialLoginViewModel> logger)
        {
            IdentityService = identityService ??
                throw new ArgumentNullException(nameof(identityService));

            UserDialogs = userDialogs ??
                throw new ArgumentNullException(nameof(userDialogs));
            _configuration = configuration;
            _navigationService = navigationService ??
                throw new ArgumentNullException(nameof(navigationService));

            Logger = logger;
        }

        protected IIdentityService IdentityService { get; }

        protected ILogger<BaseSocialLoginViewModel> Logger { get; }

        private IMvxAsyncCommand _googleLoginCommand;
        public IMvxAsyncCommand GoogleLoginCommand => _googleLoginCommand ??= new MvxAsyncCommand(DoGoogleLoginCommand);

        private IMvxAsyncCommand _facebookLoginCommand;
        public IMvxAsyncCommand FacebookLoginCommand => _facebookLoginCommand ??= new MvxAsyncCommand(DoFacebookLoginCommand);

        private async Task DoGoogleLoginCommand()
        {
            await DoSocialLoginAsync(ExternalLoginProvider.Google);
        }

        private async Task DoFacebookLoginCommand()
        {
            await DoSocialLoginAsync(ExternalLoginProvider.Facebook);
        }

        private async Task DoSocialLoginAsync(ExternalLoginProvider loginProvider)
        {
            IsLoading = true;

            try
            {
                await IdentityService.PerformSocialLoginSignInAsync(loginProvider)
                    .ConfigureAwait(false);

                await _navigationService.Navigate<HomeViewModel>(
                    new MvxBundle(new Dictionary<string, string>
                    {
                        {MvxBundleConstaints.ClearBackStack, ""}
                    })).ConfigureAwait(true);
            }
            catch (TaskCanceledException)
            { }
            catch (Exception ex)
            {
                Logger?.LogCritical(
                    ex,
                    "Error while loggin using social login. Provider: {provider}",
                    loginProvider);

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
