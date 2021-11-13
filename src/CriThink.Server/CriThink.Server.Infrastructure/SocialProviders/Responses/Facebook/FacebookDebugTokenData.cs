using System.Text.Json.Serialization;

namespace CriThink.Server.Infrastructure.SocialProviders
{
    internal class FacebookDebugTokenData
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("application")]
        public string Application { get; set; }

        [JsonPropertyName("is_valid")]
        public bool IsValid { get; set; }

        [JsonPropertyName("user_id")]
        public string UserId { get; set; }
    }
}
