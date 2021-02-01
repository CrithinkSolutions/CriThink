using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using CriThink.Client.Core.Services;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using CriThink.Common.Helpers;
using MvvmCross.Logging;

namespace CriThink.Client.Core.ViewModels.Users
{
    public class BaseSocialLoginViewModel : BaseViewModel
    {
        private readonly IUserDialogs _userDialogs;

        public BaseSocialLoginViewModel(IIdentityService identityService, IUserDialogs userDialogs, IMvxLogProvider logProvider)
        {
            IdentityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _userDialogs = userDialogs ?? throw new ArgumentNullException(nameof(userDialogs));
            Log = logProvider?.GetLogFor<BaseSocialLoginViewModel>();
        }

        protected IIdentityService IdentityService { get; }

        protected IMvxLog Log { get; }

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
            }
            catch (Exception ex)
            {
                Log?.FatalException("Error while loggin using social login", ex, string.IsNullOrWhiteSpace(token), loginProvider);
                await ShowErrorMessage($"An error occurred when logging in with {loginProvider}").ConfigureAwait(true);
            }
            finally
            {
                IsLoading = false;
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
