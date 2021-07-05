using System;
using System.Threading.Tasks;
using CriThink.Client.Core.Services;
using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace CriThink.Client.Core.ViewModels.Statistics
{
    public class HomeStatisticsViewModel : BaseBottomViewViewModel
    {
        private readonly IPlatformService _platformService;
        private bool _isInitialized;

        public HomeStatisticsViewModel(IPlatformService platformService, IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            TabId = "focus";
            _platformService = platformService ?? throw new ArgumentNullException(nameof(platformService));
        }

        #region Properties

        private long _platformUsers;
        public string PlatformUsers
        {
            get => $"{_platformUsers:000,000}";
            set
            {
                if (long.TryParse(value, out var strValue))
                    SetProperty(ref _platformUsers, strValue);
            }
        }

        private long _platformSearches;
        public string PlatformSearches
        {
            get => $"{_platformSearches:000,000}";
            set
            {
                if (long.TryParse(value, out var strValue))
                    SetProperty(ref _platformSearches, strValue);
            }
        }

        private long _userSearches;
        public string UserSearches
        {
            get => $"{_userSearches:000,000}";
            set
            {
                if (long.TryParse(value, out var strValue))
                    SetProperty(ref _userSearches, strValue);
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
