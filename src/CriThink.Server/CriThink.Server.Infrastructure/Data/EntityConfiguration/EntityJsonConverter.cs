using System.Text.Json;

namespace CriThink.Server.Infrastructure.Data.EntityConfiguration
{
    internal static class EntityJsonConverter
    {
        public static T GetFromJson<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json);
        }

        public static string ToJson(object value)
        {
            return JsonSerializer.Serialize(value);
        }
    }
}
