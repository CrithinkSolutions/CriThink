using System;
using System.Threading.Tasks;
using CriThink.Client.Core.Data.Settings;

namespace CriThink.Client.Core.Services
{
    public class ApplicationService : IApplicationService
    {
        private const string ApplicationFirstStart = "application_firstStart";

        private readonly ISettingsRepository _settingsRepository;

        public ApplicationService(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository ?? throw new ArgumentNullException(nameof(settingsRepository));
        }

        public bool IsFirstStart()
        {
            var isFirstStart = _settingsRepository.ContainsPreference(ApplicationFirstStart);
            return !isFirstStart;
        }

        public async Task SetFirstAppStartAsync()
        {
            await _settingsRepository.SavePreferenceAsync(ApplicationFirstStart, bool.TrueString, false).ConfigureAwait(false);
        }
    }

    public interface IApplicationService
    {
        bool IsFirstStart();

        Task SetFirstAppStartAsync();
    }
}
