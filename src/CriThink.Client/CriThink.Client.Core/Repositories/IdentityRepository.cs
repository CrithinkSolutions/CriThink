using System;
using System.Text.Json;
using System.Threading.Tasks;
using CriThink.Client.Core.Data.Settings;
using CriThink.Client.Core.Models.Identity;
using MvvmCross.Logging;

namespace CriThink.Client.Core.Repositories
{
    public class IdentityRepository : IIdentityRepository
    {
        private const string UserProfile = "user_profile";

        private readonly ISettingsRepository _settingsRepository;
        private readonly IMvxLog _log;

        public IdentityRepository(ISettingsRepository settingsRepository, IMvxLogProvider logProvider)
        {
            _settingsRepository = settingsRepository ?? throw new ArgumentNullException(nameof(settingsRepository));
            _log = logProvider?.GetLogFor<IdentityRepository>();
        }

        public async Task<UserAccess> GetUserAccessAsync()
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    IncludeFields = true,
                };

                var serializedData = await GetUserInSettingsSettingAsync(UserProfile);
                return string.IsNullOrWhiteSpace(serializedData) ?
                    null :
                    JsonSerializer.Deserialize<UserAccess>(serializedData, options);
            }
            catch (Exception ex)
            {
                _log?.FatalException("Error getting user access info", ex);
                throw;
            }
        }

        public async Task SetUserAccessAsync(UserAccess userAccess)
        {
            if (userAccess is null)
                return;

            try
            {
                var serializedData = JsonSerializer.Serialize(userAccess);
                await UpdateUserInSettingsAsync(UserProfile, serializedData);
            }
            catch (Exception ex)
            {
                _log.ErrorException("Error saving user access info", ex);
                throw;
            }
        }

        public void EraseUserInfo()
        {
            try
            {
                EraseUserSettingsAsync(UserProfile);
            }
            catch (Exception ex)
            {
                _log?.ErrorException("Error deleting user info", ex);
                throw;
            }
        }

        private Task UpdateUserInSettingsAsync(string key, string value) => _settingsRepository.SavePreferenceAsync(key, value, true);

        private Task<string> GetUserInSettingsSettingAsync(string key, string defaultValue = "") => _settingsRepository.GetPreferenceAsync(key, defaultValue, true);

        private void EraseUserSettingsAsync(string key) => _settingsRepository.RemovePreference(key, true);
    }
}
