using Acr.UserDialogs;
using CriThink.Client.Core.Services;
using CriThink.Client.Core.ViewModels.Users;
using Microsoft.Extensions.Logging;
using MvvmCross.Navigation;

namespace CriThink.Client.Core.ViewModels
{
    public class WelcomeViewModel : BaseViewModel
    {
        public WelcomeViewModel(IMvxNavigationService navigationService, IIdentityService identityService, IPlatformService platformService, IUserDialogs userDialogs, ILogger<BaseSocialLoginViewModel> logger)
        {
            SignUpViewModel = new SignUpViewModel(navigationService, identityService, userDialogs, logger);
            WelcomeLoginSignInViewModel = new WelcomeLoginSignInViewModel(navigationService, platformService);
        }

        public SignUpViewModel SignUpViewModel { get; }

        public WelcomeLoginSignInViewModel WelcomeLoginSignInViewModel { get; }
    }
}
