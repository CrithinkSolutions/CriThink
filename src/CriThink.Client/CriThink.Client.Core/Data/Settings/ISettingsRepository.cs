using System.Threading.Tasks;

namespace CriThink.Client.Core.Data.Settings
{
    public interface ISettingsRepository
    {
        Task SavePreferenceAsync(string key, string value, bool isSensitive);

        Task<string> GetPreferenceAsync(string key, string defaultValue, bool isSensitive);

        bool ContainsPreference(string key);

        void RemovePreference(string key, bool isSensitive);

        void RemoveAllPreferences(bool areSensitive);
    }
}
