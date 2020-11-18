using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace CriThink.Client.Core.ViewModels.DebunkingNews
{
    public class DebunkingNewsViewModel : BaseBottomViewViewModel
    {
        public DebunkingNewsViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            TabId = "debunking_news";
        }
    }
}
