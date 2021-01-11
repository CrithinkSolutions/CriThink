using System;
using CriThink.Client.Core.Platform;
using Microsoft.Extensions.Logging;

namespace CriThink.Client.Core.Services
{
    public class PlatformService : IPlatformService
    {
        private const string FacebookPageId = "111916197191315";
        private const string InstagramProfileName = "crithink.solutions";
        private const string TwitterProfileName = "1262643502380470273";
        private const string LinkedInProfile = "company/crithink-solutions/";

        private readonly IPlatformDetails _platformDetails;
        private readonly ILogger<PlatformService> _logger;

        public PlatformService(IPlatformDetails platformDetails, ILogger<PlatformService> logger)
        {
            _platformDetails = platformDetails ?? throw new ArgumentNullException(nameof(platformDetails));
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
                _logger?.LogError(ex, "Failed to open Facebook app", FacebookPageId);
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
                _logger?.LogError(ex, "Failed to open Instagram app", InstagramProfileName);
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
                _logger?.LogError(ex, "Failed to open Twitter app", TwitterProfileName);
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
                _logger?.LogError(ex, "Failed to open LinkedIn app", LinkedInProfile);
            }
        }
    }

    public interface IPlatformService
    {
        void OpenCriThinkFacebookPage();

        void OpenCriThinkInstagramProfile();

        void OpenCriThinkTwitterProfile();

        void OpenCriThinkLinkedInProfile();
    }
}
