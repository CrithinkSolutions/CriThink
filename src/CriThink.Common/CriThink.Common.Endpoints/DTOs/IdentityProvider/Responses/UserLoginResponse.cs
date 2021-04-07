using System.Text.Json.Serialization;
// ReSharper disable CheckNamespace

namespace CriThink.Common.Endpoints.DTOs.IdentityProvider
{
    public class UserLoginResponse
    {
        [JsonPropertyName("token")]
        public JwtTokenResponse JwtToken { get; set; }

        [JsonPropertyName("userId")]
        public string UserId { get; set; }

        [JsonPropertyName("userEmail")]
        public string UserEmail { get; set; }

        [JsonPropertyName("username")]
        public string UserName { get; set; }

        [JsonPropertyName("avatarPath")]
        public string AvatarPath { get; set; }
    }
}
