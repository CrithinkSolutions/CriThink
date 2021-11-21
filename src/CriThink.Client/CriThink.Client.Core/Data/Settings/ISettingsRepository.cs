using System.Threading.Tasks;

namespace CriThink.Client.Core.Data.Settings
{
    public interface ISettingsRepository
    {
        Task SavePreferenceAsync(string key, string value, bool isSensitive);

        void SavePreference(string key, int value);

        Task<string> GetPreferenceAsync(string key, string defaultValue, bool isSensitive);

        int GetPreference(string key, int defaultValue);

        bool ContainsPreference(string key);

        void RemovePreference(string key, bool isSensitive);

        void RemoveAllPreferences(bool areSensitive);
    }
}
