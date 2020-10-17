﻿using CriThink.Client.Core.ViewModels.Users;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace CriThink.Client.Core.ViewModels
{
    public class WelcomeViewModel : MvxViewModel
    {
        public WelcomeViewModel(IMvxNavigationService navigationService)
        {
            SignUpViewModel = new SignUpViewModel(navigationService);
            WelcomeLoginSignInViewModel = new WelcomeLoginSignInViewModel(navigationService);
        }

        public SignUpViewModel SignUpViewModel { get; }

        public WelcomeLoginSignInViewModel WelcomeLoginSignInViewModel { get; }
    }
}