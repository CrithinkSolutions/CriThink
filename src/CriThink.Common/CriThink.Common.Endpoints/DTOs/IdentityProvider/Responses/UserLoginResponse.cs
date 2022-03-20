using System.Text.Json.Serialization;
// ReSharper disable CheckNamespace

namespace CriThink.Common.Endpoints.DTOs.IdentityProvider
{
    public class UserLoginResponse
    {
        [JsonPropertyName("token")]
        public JwtTokenResponse JwtToken { get; set; }

        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }

        [JsonIgnore]
        public bool Succeed =>
            !string.IsNullOrWhiteSpace(RefreshToken) &&
            JwtToken is not null;
    }
}
