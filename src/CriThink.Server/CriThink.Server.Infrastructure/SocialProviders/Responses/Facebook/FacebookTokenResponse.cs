using System.Text.Json.Serialization;

namespace CriThink.Server.Infrastructure.SocialProviders
{
    internal class FacebookTokenResponse
    {
        [JsonPropertyName("data")]
        public FacebookDebugTokenData Data { get; set; }
    }
}
