using System.Text.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace CriThink.Server.Domain.Models.DTOs.Facebook
{
    public class FacebookTokenResponse
    {
        [JsonPropertyName("data")]
        public DebugTokenData Data { get; set; }
    }

    public class DebugTokenData
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
