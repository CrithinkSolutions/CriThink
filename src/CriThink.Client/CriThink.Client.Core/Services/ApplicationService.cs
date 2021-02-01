using System;
using System.Threading.Tasks;
using CriThink.Client.Core.Data.Settings;
using MvvmCross.Logging;

namespace CriThink.Client.Core.Services
{
    public class ApplicationService : IApplicationService
    {
        private const string ApplicationFirstStart = "application_firstStart";

        private readonly ISettingsRepository _settingsRepository;
        private readonly IMvxLog _log;

        public ApplicationService(ISettingsRepository settingsRepository, IMvxLogProvider logProvider)
        {
            _settingsRepository = settingsRepository ?? throw new ArgumentNullException(nameof(settingsRepository));
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
    }

    public interface IApplicationService
    {
        bool IsFirstStart();

        Task SetFirstAppStartAsync();
    }
}
