using System;
using System.Threading.Tasks;
using MvvmCross.Logging;
using Xamarin.Essentials;

namespace CriThink.Client.Core.Data.Settings
{
    public class SettingsRepository : ISettingsRepository
    {
        private readonly SecureSettingsRepository _secureSettingsRepository;
        private readonly IMvxLog _logger;

        public SettingsRepository(SecureSettingsRepository secureSettingsRepository, IMvxLog logger)
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
                _logger?.Log(MvxLogLevel.Error, () => "Error saving a preference", ex, key);
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
                _logger?.Log(MvxLogLevel.Error, () => "Error reading preference", ex, key, defaultValue);
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
                _logger?.Log(MvxLogLevel.Error, () => "Error checking key existance", ex, key);
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
                    _logger?.Log(MvxLogLevel.Error, () => "Error removing a preference", ex, key);
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
                    _logger?.Log(MvxLogLevel.Error, () => "Error clearning all preferences", ex);
                    throw;
                }
            }
        }
    }
}
