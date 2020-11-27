using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace CriThink.Client.Core.ViewModels.Games
{
    public class HomeGameViewModel : BaseBottomViewViewModel
    {
        public HomeGameViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            TabId = "play_games";
        }
    }
}
