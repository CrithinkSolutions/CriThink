using Microsoft.Extensions.Logging;
using MvvmCross.Navigation;

namespace CriThink.Client.Core.ViewModels.SpotFakeNews
{
    public class SpotFakeNewsHomeViewModel : BaseBottomViewViewModel
    {
        public SpotFakeNewsHomeViewModel(ILogger<SpotFakeNewsHomeViewModel> logger, IMvxNavigationService navigationService)
            : base(logger, navigationService)
        {
            TabId = "spot_fakenews";
        }
    }
}
