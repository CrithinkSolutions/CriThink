using System;
using System.Threading.Tasks;
using CriThink.Client.Core.Api;
using CriThink.Client.Core.Data.Settings;
using Microsoft.Extensions.Logging;
using Plugin.StoreReview;

namespace CriThink.Client.Core.Services
{
    public class ApplicationService : IApplicationService
    {
        private const string ApplicationStartCounterKey = "start_counter";
        private const string HasAppReviewKey = "has_app_review";

        private readonly ISettingsRepository _settingsRepository;
        private readonly IServiceApi _serviceApi;
        private readonly ILogger<ApplicationService> _logger;

        public ApplicationService(ISettingsRepository settingsRepository, IServiceApi serviceApi, ILogger<ApplicationService> logger)
        {
            _settingsRepository = settingsRepository ?? throw new ArgumentNullException(nameof(settingsRepository));
            _serviceApi = serviceApi ?? throw new ArgumentNullException(nameof(serviceApi));
            _logger = logger;
        }

        public bool IsFirstStart()
        {
            var count = GetApplicationStartCounter();
            return count == 0;
        }

        public void IncrementAppStartCounter()
        {
            var count = GetApplicationStartCounter();
            _settingsRepository.SavePreference(ApplicationStartCounterKey, ++count);
        }

        public async Task<bool> CanAppStartAsync()
        {
            try
            {
                await _serviceApi.IsAppEnabledAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogCritical(ex, "Error getting the app enabled flag");
                return false;
            }
        }

        public async Task AskForStoreReviewAsync()
        {
            var testMode = false;
#if DEBUG
            testMode = true;
#endif
            if (CrossStoreReview.IsSupported)
                await CrossStoreReview.Current.RequestReview(testMode);

            await _settingsRepository.SavePreferenceAsync(
                HasAppReviewKey,
                bool.TrueString,
                false);
        }

        public async Task<bool> ShouldAskForStoreReviewAsync()
        {
            var hasAppReview = await HasAppReviewAsync();
            if (hasAppReview)
                return false;

            var count = _settingsRepository.GetPreference(ApplicationStartCounterKey, 0);
            return count % 5 == 0;
        }

        private async Task<bool> HasAppReviewAsync()
        {
            var hasReview = await _settingsRepository.GetPreferenceAsync(
                HasAppReviewKey,
                bool.FalseString,
                false);

            return bool.Parse(hasReview);
        }

        private int GetApplicationStartCounter()
        {
            return _settingsRepository.GetPreference(ApplicationStartCounterKey, 0);
        }
    }

    public interface IApplicationService
    {
        bool IsFirstStart();

        void IncrementAppStartCounter();

        Task<bool> ShouldAskForStoreReviewAsync();

        Task<bool> CanAppStartAsync();

        Task AskForStoreReviewAsync();
    }
}
