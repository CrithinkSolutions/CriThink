using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Xamarin.Essentials;

namespace CriThink.Client.Core.Data.Settings
{
    public class SettingsRepository : ISettingsRepository
    {
        private readonly SecureSettingsRepository _secureSettingsRepository;
        private readonly ILogger<SettingsRepository> _logger;

        public SettingsRepository(SecureSettingsRepository secureSettingsRepository, ILogger<SettingsRepository> logger)
        {
            _secureSettingsRepository = secureSettingsRepository;
            _logger = logger;
        }

        public Task SavePreferenceAsync(string key, string value, bool isSensitive)
        {
            if (isSensitive)
            {
                return _secureSettingsRepository.SavePreferenceAsync(key, value);
            }

            try
            {
                Preferences.Set(key, value);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error saving a preference", key);
                throw;
            }

            return Task.CompletedTask;
        }

        public Task<string> GetPreferenceAsync(string key, string defaultValue, bool isSensitive)
        {
            if (isSensitive)
            {
                return _secureSettingsRepository.GetPreferenceAsync(key, defaultValue);
            }

            try
            {
                var value = Preferences.Get(key, defaultValue);
                return Task.FromResult(value);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error reading preference", key, defaultValue);
                throw;
            }
        }

        public bool ContainsPreference(string key)
        {
            try
            {
                return Preferences.ContainsKey(key);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error checking key existance", key);
                throw;
            }
        }

        public void RemovePreference(string key, bool isSensitive)
        {
            if (isSensitive)
            {
                _secureSettingsRepository.RemovePreference(key);
            }
            else
            {
                try
                {
                    Preferences.Remove(key);
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Error removing a preference", key);
                    throw;
                }
            }
        }

        public void RemoveAllPreferences(bool areSensitive)
        {
            if (areSensitive)
            {
                _secureSettingsRepository.RemoveAllPreferences();
            }
            else
            {
                try
                {
                    Preferences.Clear();
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Error clearning all preferences");
                    throw;
                }
            }
        }
    }
}
