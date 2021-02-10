using Acr.UserDialogs;
using CriThink.Client.Core.Services;
using CriThink.Client.Core.ViewModels.Users;
using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace CriThink.Client.Core.ViewModels
{
    public class WelcomeViewModel : BaseViewModel
    {
        public WelcomeViewModel(IMvxNavigationService navigationService, IIdentityService identityService, IPlatformService platformService, IUserDialogs userDialogs, IMvxLogProvider logProvider)
        {
            SignUpViewModel = new SignUpViewModel(navigationService, identityService, userDialogs, logProvider);
            WelcomeLoginSignInViewModel = new WelcomeLoginSignInViewModel(navigationService, platformService);
        }

        public SignUpViewModel SignUpViewModel { get; }

        public WelcomeLoginSignInViewModel WelcomeLoginSignInViewModel { get; }
    }
}
