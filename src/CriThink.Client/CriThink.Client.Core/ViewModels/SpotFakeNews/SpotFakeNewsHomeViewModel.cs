using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace CriThink.Client.Core.ViewModels.SpotFakeNews
{
    public class SpotFakeNewsHomeViewModel : BaseBottomViewViewModel
    {
        public SpotFakeNewsHomeViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            TabId = "spot_fakenews";
        }
    }
}
