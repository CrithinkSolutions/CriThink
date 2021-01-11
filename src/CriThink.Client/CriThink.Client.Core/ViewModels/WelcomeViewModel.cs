﻿using CriThink.Client.Core.Services;
using CriThink.Client.Core.ViewModels.Users;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace CriThink.Client.Core.ViewModels
{
    public class WelcomeViewModel : MvxViewModel
    {
        public WelcomeViewModel(IMvxNavigationService navigationService, IIdentityService identityService, IPlatformService platformService)
        {
            SignUpViewModel = new SignUpViewModel(navigationService, identityService);
            WelcomeLoginSignInViewModel = new WelcomeLoginSignInViewModel(navigationService, platformService);
        }

        public SignUpViewModel SignUpViewModel { get; }

        public WelcomeLoginSignInViewModel WelcomeLoginSignInViewModel { get; }
    }
}
