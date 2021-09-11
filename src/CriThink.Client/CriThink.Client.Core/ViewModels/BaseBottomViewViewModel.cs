using Microsoft.Extensions.Logging;
using MvvmCross.Navigation;

namespace CriThink.Client.Core.ViewModels
{
    /// <summary>
    /// Base class for view models used in the bottom nav bar
    /// </summary>
    public abstract class BaseBottomViewViewModel : BaseViewModel
    {
        protected BaseBottomViewViewModel(ILogger<BaseBottomViewViewModel> logger, IMvxNavigationService navigationService)
        {
            Logger = logger;
            NavigationService = navigationService;
        }

        public ILogger<BaseBottomViewViewModel> Logger { get; }

        public IMvxNavigationService NavigationService { get; }

        public string TabId { get; protected set; }
    }
}
