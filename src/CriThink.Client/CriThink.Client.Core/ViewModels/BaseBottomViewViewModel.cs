using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace CriThink.Client.Core.ViewModels
{
    /// <summary>
    /// Base class for view models used in the bottom nav bar
    /// </summary>
    public abstract class BaseBottomViewViewModel : BaseViewModel
    {
        protected BaseBottomViewViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
        {
            LogProvider = logProvider;
            NavigationService = navigationService;
        }

        public IMvxLogProvider LogProvider { get; }

        public IMvxNavigationService NavigationService { get; }

        public string TabId { get; protected set; }
    }
}
