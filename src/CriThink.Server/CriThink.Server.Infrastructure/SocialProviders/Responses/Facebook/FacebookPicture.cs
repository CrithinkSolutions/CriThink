using System.Text.Json.Serialization;

namespace CriThink.Server.Infrastructure.SocialProviders
{
    internal class FacebookPicture
    {
        [JsonPropertyName("data")]
        public FacebookUserInfoData Data { get; set; }
    }
}
