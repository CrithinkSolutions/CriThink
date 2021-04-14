using System;
using System.Threading.Tasks;
using CriThink.Client.Core.Api;
using CriThink.Client.Core.Data.Settings;
using MvvmCross.Logging;

namespace CriThink.Client.Core.Services
{
    public class ApplicationService : IApplicationService
    {
        private const string ApplicationFirstStart = "application_firstStart";

        private readonly ISettingsRepository _settingsRepository;
        private readonly IServiceApi _serviceApi;
        private readonly IMvxLog _log;

        public ApplicationService(ISettingsRepository settingsRepository, IServiceApi serviceApi, IMvxLogProvider logProvider)
        {
            _settingsRepository = settingsRepository ?? throw new ArgumentNullException(nameof(settingsRepository));
            _serviceApi = serviceApi ?? throw new ArgumentNullException(nameof(serviceApi));
            _log = logProvider?.GetLogFor<ApplicationService>();
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
                _log?.FatalException("Error setting first app start", ex);
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
                _log?.FatalException("Error getting the app enabled flag", ex);
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
