using System;
using System.Threading.Tasks;
using CriThink.Client.Core.Services;
using Microsoft.Extensions.Logging;
using MvvmCross.Navigation;

namespace CriThink.Client.Core.ViewModels.Statistics
{
    public class HomeStatisticsViewModel : BaseBottomViewViewModel
    {
        private readonly IPlatformService _platformService;
        private bool _isInitialized;

        public HomeStatisticsViewModel(IPlatformService platformService, ILogger<HomeStatisticsViewModel> logger, IMvxNavigationService navigationService)
            : base(logger, navigationService)
        {
            TabId = "focus";
            _platformService = platformService ?? throw new ArgumentNullException(nameof(platformService));
        }

        #region Properties

        private long _platformUsers;
        public long PlatformUsers
        {
            get => _platformUsers;
            set
            {
                SetProperty(ref _platformUsers, value);
            }
        }

        private long _platformSearches;
        public long PlatformSearches
        {
            get => _platformSearches;
            set
            {
                SetProperty(ref _platformSearches, value);
            }
        }

        private long _userSearches;
        public long UserSearches
        {
            get => _userSearches;
            set
            {
                SetProperty(ref _userSearches, value);
            }
        }

        #endregion

        public override async Task Initialize()
        {
            await base.Initialize();

            await GetPlatformDataUsageAsync();
        }

        private async Task GetPlatformDataUsageAsync()
        {
            if (_isInitialized && !HasFailed)
                return;

            try
            {
                HasFailed = false;
                IsLoading = true;
                var result = await _platformService.GetPlatformDataUsageAsync(default);

                _platformUsers = result.PlatformUsers;
                _platformSearches = result.PlatformSearches;
                _userSearches = result.UserSearches;
            }
            catch (Exception)
            {
                HasFailed = true;
            }
            finally
            {
                _isInitialized = true;
                IsLoading = false;
                await RaiseAllPropertiesChanged();
            }
        }
    }
}
