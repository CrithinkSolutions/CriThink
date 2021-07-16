using System;
using System.Text.Json;
using System.Threading.Tasks;
using CriThink.Client.Core.Data.Settings;
using CriThink.Client.Core.Models.Identity;
using Microsoft.Extensions.Logging;

namespace CriThink.Client.Core.Repositories
{
    public class IdentityRepository : IIdentityRepository
    {
        private const string UserProfile = "user_profile";

        private readonly ISettingsRepository _settingsRepository;
        private readonly ILogger<IdentityRepository> _logger;

        public IdentityRepository(ISettingsRepository settingsRepository, ILogger<IdentityRepository> logger)
        {
            _settingsRepository = settingsRepository ?? throw new ArgumentNullException(nameof(settingsRepository));
            _logger = logger;
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
                _logger?.LogCritical(ex, "Error getting user access info");
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
                _logger?.LogError(ex, "Error saving user access info");
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
                _logger?.LogError(ex, "Error deleting user info");
                throw;
            }
        }

        private Task UpdateUserInSettingsAsync(string key, string value) => _settingsRepository.SavePreferenceAsync(key, value, true);

        private Task<string> GetUserInSettingsSettingAsync(string key, string defaultValue = "") => _settingsRepository.GetPreferenceAsync(key, defaultValue, true);

        private void EraseUserSettingsAsync(string key) => _settingsRepository.RemovePreference(key, true);
    }
}
