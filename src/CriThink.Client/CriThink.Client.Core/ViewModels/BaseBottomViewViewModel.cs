using MvvmCross.ViewModels;

namespace CriThink.Client.Core.ViewModels
{
    /// <summary>
    /// Base class for view models used in the bottom nav bar
    /// </summary>
    public abstract class BaseBottomViewViewModel : MvxViewModel
    {
        public string TabId { get; protected set; }
    }
}
