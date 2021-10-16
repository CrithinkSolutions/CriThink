using CriThink.Client.Core.Services;
using CriThink.Client.Core.ViewModels.Users;
using MvvmCross.Navigation;

namespace CriThink.Client.Core.ViewModels
{
    public class WelcomeViewModel : BaseViewModel
    {
        public WelcomeViewModel(
            IMvxNavigationService navigationService,
            IPlatformService platformService)
        {
            WelcomeLoginSignInViewModel = new WelcomeLoginSignInViewModel(
                navigationService,
                platformService);
        }

        public WelcomeLoginSignInViewModel WelcomeLoginSignInViewModel { get; }
    }
}
