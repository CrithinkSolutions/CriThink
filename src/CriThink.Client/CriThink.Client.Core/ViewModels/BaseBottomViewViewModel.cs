using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace CriThink.Client.Core.ViewModels
{
    /// <summary>
    /// Base class for view models used in the bottom nav bar
    /// </summary>
    public abstract class BaseBottomViewViewModel : MvxNavigationViewModel
    {
        protected BaseBottomViewViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        { }

        public string TabId { get; protected set; }
    }
}
