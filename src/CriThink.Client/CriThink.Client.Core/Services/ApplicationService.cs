using System;
using System.Threading.Tasks;
using CriThink.Client.Core.Api;
using CriThink.Client.Core.Data.Settings;
using Microsoft.Extensions.Logging;

namespace CriThink.Client.Core.Services
{
    public class ApplicationService : IApplicationService
    {
        private const string ApplicationFirstStart = "application_firstStart";

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
            var isFirstStart = _settingsRepository.ContainsPreference(ApplicationFirstStart);
            return !isFirstStart;
        }

        public async Task SetFirstAppStartAsync()
        {
            try
            {
                await _settingsRepository.SavePreferenceAsync(ApplicationFirstStart, bool.TrueString, false)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger?.LogCritical(ex, "Error setting first app start");
            }
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
    }

    public interface IApplicationService
    {
        bool IsFirstStart();

        Task SetFirstAppStartAsync();

        Task<bool> CanAppStartAsync();
    }
}
