using System;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace CriThink.Client.Core.ViewModels.Users
{
    public class SignUpEmailViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public SignUpEmailViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        }
    }
}
