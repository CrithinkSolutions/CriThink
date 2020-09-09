using System.Text.Json.Serialization;

// ReSharper disable CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.IdentityProvider
{
    public class VerifyUserEmailResponse
    {
        [JsonPropertyName("jwtToken")]
        public JwtTokenResponse JwtToken { get; set; }

        [JsonPropertyName("userId")]
        public string UserId { get; set; }

        [JsonPropertyName("userEmail")]
        public string UserEmail { get; set; }
    }
}
