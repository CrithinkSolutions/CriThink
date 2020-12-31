using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CriThink.Client.Core.Services;
using CriThink.Client.Core.ViewModels.DebunkingNews;
using CriThink.Client.Core.ViewModels.Games;
using CriThink.Client.Core.ViewModels.NewsChecker;
using CriThink.Client.Core.ViewModels.SpotFakeNews;
using CriThink.Client.Core.ViewModels.Users;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace CriThink.Client.Core.ViewModels
{
    public class HomeViewModel : MvxNavigationViewModel
    {
        private readonly IIdentityService _identityService;

        public HomeViewModel(IMvxNavigationService navigationService, IMvxLogProvider logProvider, IIdentityService identityService)
            : base(logProvider, navigationService)
        {
            var tabs = new List<BaseBottomViewViewModel>
            {
                Mvx.IoCProvider.IoCConstruct<NewsCheckerViewModel>(),
                Mvx.IoCProvider.IoCConstruct<DebunkingNewsViewModel>(),
                Mvx.IoCProvider.IoCConstruct<SpotFakeNewsHomeViewModel>(),
                Mvx.IoCProvider.IoCConstruct<HomeGameViewModel>()
            };

            BottomViewTabs = tabs;

            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }

        #region Properties

        public List<BaseBottomViewViewModel> BottomViewTabs { get; }

        private IMvxCommand<string> _bottomNavigationItemSelectedCommand;
        public IMvxCommand<string> BottomNavigationItemSelectedCommand => _bottomNavigationItemSelectedCommand ??= new MvxCommand<string>(DoBottomNavigationItemSelectedCommand);

        #endregion

        public override async Task Initialize()
        {
            await base.Initialize().ConfigureAwait(false);

            var user = await _identityService.GetLoggedUserAsync().ConfigureAwait(false);
            if (user is null)
            {
                await NavigationService.Navigate<SignUpViewModel>().ConfigureAwait(true);
            }
        }

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
