using System;
using System.Threading.Tasks;
using CriThink.Client.Core.Services;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using CriThink.Common.Helpers;
using MvvmCross.ViewModels;

namespace CriThink.Client.Core.ViewModels.Users
{
    public class BaseSocialLoginViewModel : MvxViewModel
    {
        public BaseSocialLoginViewModel(IIdentityService identityService)
        {
            IdentityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }

        protected IIdentityService IdentityService { get; }

        public async Task PerformLoginSignAsync(string token, ExternalLoginProvider loginProvider)
        {
            var request = new ExternalLoginProviderRequest
            {
                SocialProvider = loginProvider,
                UserToken = Base64Helper.ToBase64(token),
            };

            await IdentityService.PerformSocialLoginSignInAsync(request).ConfigureAwait(false);
        }
    }
}
