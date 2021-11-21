using System.Text.Json.Serialization;

namespace CriThink.Server.Infrastructure.SocialProviders
{
    internal class FacebookUserInfoDetail
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonPropertyName("picture")]
        public FacebookPicture Picture { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
