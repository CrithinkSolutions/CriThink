using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Api;
using CriThink.Client.Core.Models.Statistics;
using CriThink.Client.Core.Platform;
using Microsoft.Extensions.Logging;
using Xamarin.Essentials;

namespace CriThink.Client.Core.Services
{
    public class PlatformService : IPlatformService
    {
        private const string FacebookPageId = "111916197191315";
        private const string InstagramProfileName = "crithink.solutions";
        private const string TwitterProfileName = "1262643502380470273";
        private const string LinkedInProfile = "company/crithink-solutions/";

        private readonly IPlatformDetails _platformDetails;
        private readonly IStatisticsApi _statisticsApi;
        private readonly ILogger<PlatformService> _logger;

        public PlatformService(IPlatformDetails platformDetails, IStatisticsApi statisticsApi, ILogger<PlatformService> logger)
        {
            _platformDetails = platformDetails ?? throw new ArgumentNullException(nameof(platformDetails));
            _statisticsApi = statisticsApi ?? throw new ArgumentNullException(nameof(statisticsApi));
            _logger = logger;
        }

        public void OpenCriThinkFacebookPage()
        {
            try
            {
                _platformDetails.OpenFacebook(FacebookPageId);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to open Facebook page", FacebookPageId);
            }
        }

        public void OpenCriThinkInstagramProfile()
        {
            try
            {
                _platformDetails.OpenInstagramProfile(InstagramProfileName);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to open Instagram profile", InstagramProfileName);
            }
        }

        public void OpenCriThinkTwitterProfile()
        {
            try
            {
                _platformDetails.OpenTwitterProfile(TwitterProfileName);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to open Twitter profile", TwitterProfileName);
            }
        }

        public void OpenCriThinkLinkedInProfile()
        {
            try
            {
                _platformDetails.OpenLinkedInProfile(LinkedInProfile);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to open LinkedIn profile", LinkedInProfile);
            }
        }

        public async Task OpenInternalBrowser(Uri uri)
        {
            try
            {
                await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "Error opening internal browser");
            }
        }

        public void OpenSkype(string profileName)
        {
            try
            {
                _platformDetails.OpenSkypeProfile(profileName);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to open a Skype profile", profileName);
            }
        }

        public async Task<StatisticsDetailModel> GetPlatformDataUsageAsync(CancellationToken cancellationToken)
        {
            try
            {
                var platformStatistics = await _statisticsApi.GetPlatformUsageDataAsync(cancellationToken);
                var userStatistics = await _statisticsApi.GetTotalUserSearchesAsync(cancellationToken);

                return new StatisticsDetailModel(platformStatistics.PlatformUsers, platformStatistics.PlatformSearches, userStatistics.UserSearches);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error getting platform data usage");
                throw;
            }
        }
    }

    public interface IPlatformService
    {
        void OpenCriThinkFacebookPage();

        void OpenCriThinkInstagramProfile();

        void OpenCriThinkTwitterProfile();

        void OpenCriThinkLinkedInProfile();

        Task OpenInternalBrowser(Uri uri);

        void OpenSkype(string profileName);

        Task<StatisticsDetailModel> GetPlatformDataUsageAsync(CancellationToken cancellationToken);
    }
}
