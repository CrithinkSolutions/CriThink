using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Xamarin.Essentials;

namespace CriThink.Client.Core.Data.Settings
{
    public class SecureSettingsRepository
    {
        private readonly ILogger<SecureSettingsRepository> _logger;

        public SecureSettingsRepository(ILogger<SecureSettingsRepository> logger)
        {
            _logger = logger;
        }

        public async Task SavePreferenceAsync(string key, string value)
        {
            try
            {
                await SecureStorage.SetAsync(key, value).ConfigureAwait(false);
            }
            catch (Exception)
            {
                await RetryToSafelySavePreferenceAsync(key, value).ConfigureAwait(false);
            }
        }

        public async Task<string> GetPreferenceAsync(string key, string defaultValue)
        {
            try
            {
                var value = await SecureStorage.GetAsync(key).ConfigureAwait(false);
                return value ?? defaultValue;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error getting setting from secure storage", key, defaultValue);
                throw;
            }
        }

        public void RemovePreference(string key)
        {
            try
            {
                SecureStorage.Remove(key);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error removing setting from secure storage", key);
                throw;
            }
        }

        public void RemoveAllPreferences()
        {
            try
            {
                SecureStorage.RemoveAll();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error removing all secure storage");
                throw;
            }
        }

        #region Privates

        private async Task RetryToSafelySavePreferenceAsync(string key, string value)
        {
            try
            {
                SecureStorage.Remove(key);
                await SecureStorage.SetAsync(key, value).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Can't save secure preference", key);
                throw;
            }
        }

        #endregion
    }
}
