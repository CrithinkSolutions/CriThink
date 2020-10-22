using System.Collections.Generic;
using System.Linq;
using CriThink.Client.Core.ViewModels.DebunkingNews;
using CriThink.Client.Core.ViewModels.NewsChecker;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace CriThink.Client.Core.ViewModels
{
    public class HomeViewModel : MvxNavigationViewModel
    {
        public HomeViewModel(IMvxNavigationService navigationService, IMvxLogProvider logProvider, IMvxLog log)
            : base(logProvider, navigationService)
        {
            var tabs = new List<BaseBottomViewViewModel>
            {
                Mvx.IoCProvider.IoCConstruct<NewsCheckerViewModel>(),
                Mvx.IoCProvider.IoCConstruct<DebunkingNewsViewModel>()
            };

            BottomViewTabs = tabs;
        }

        #region Properties

        public List<BaseBottomViewViewModel> BottomViewTabs { get; }

        private IMvxCommand<string> _bottomNavigationItemSelectedCommand;
        public IMvxCommand<string> BottomNavigationItemSelectedCommand => _bottomNavigationItemSelectedCommand ??= new MvxCommand<string>(DoBottomNavigationItemSelectedCommand);

        #endregion

        private void DoBottomNavigationItemSelectedCommand(string tabId)
        {
            foreach (var item in BottomViewTabs.Where(item => tabId == item.TabId))
            {
                NavigationService.Navigate(item);
                break;
            }
        }
    }
}
