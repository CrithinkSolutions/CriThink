using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace CriThink.Client.Core.ViewModels.Statistics
{
    public class HomeStatisticsViewModel : BaseBottomViewViewModel
    {
        public HomeStatisticsViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            TabId = "focus";
        }

        #region Properties

        private int _totalUsers;
        public string TotalUsers
        {
            get => _totalUsers.ToString("D6");
            set
            {
                if (int.TryParse(value, out var strValue))
                    SetProperty(ref _totalUsers, strValue);
            }
        }

        private int _totalSearches;
        public string TotalSearches
        {
            get => _totalSearches.ToString("D6");
            set
            {
                if (int.TryParse(value, out var strValue))
                    SetProperty(ref _totalSearches, strValue);
            }
        }

        private int _userSearches;
        public string UserSearches
        {
            get => _userSearches.ToString("D6");
            set
            {
                if (int.TryParse(value, out var strValue))
                    SetProperty(ref _userSearches, strValue);
            }
        }

        #endregion

        public override void Prepare()
        {
            base.Prepare();
        }
    }
}
