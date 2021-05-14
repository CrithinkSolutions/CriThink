using System;
using System.Threading.Tasks;
using CriThink.Client.Core.Platform;
using MvvmCross.Logging;
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
        private readonly IMvxLog _log;

        public PlatformService(IPlatformDetails platformDetails, IMvxLogProvider logProvider)
        {
            _platformDetails = platformDetails ?? throw new ArgumentNullException(nameof(platformDetails));
            _log = logProvider?.GetLogFor<PlatformService>();
        }

        public void OpenCriThinkFacebookPage()
        {
            try
            {
                _platformDetails.OpenFacebook(FacebookPageId);
            }
            catch (Exception ex)
            {
                _log?.Error(ex, "Failed to open Facebook page", FacebookPageId);
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
                _log?.Error(ex, "Failed to open Instagram profile", InstagramProfileName);
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
                _log?.Error(ex, "Failed to open Twitter profile", TwitterProfileName);
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
                _log?.Error(ex, "Failed to open LinkedIn profile", LinkedInProfile);
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
                _log?.WarnException("Error opening internal browser", ex);
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
                _log?.Error(ex, "Failed to open a Skype profile", profileName);
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
    }
}
