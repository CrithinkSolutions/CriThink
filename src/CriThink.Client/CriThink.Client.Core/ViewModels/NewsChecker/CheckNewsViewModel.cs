using System;
using MvvmCross.Navigation;

#pragma warning disable CA1056 // URI-like properties should not be strings
namespace CriThink.Client.Core.ViewModels.NewsChecker
{
    public class CheckNewsViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public CheckNewsViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        }

        private string _newsUri;

        public string NewsUri
        {
            get => _newsUri;
            set => SetProperty(ref _newsUri, value);
        }
    }
}
