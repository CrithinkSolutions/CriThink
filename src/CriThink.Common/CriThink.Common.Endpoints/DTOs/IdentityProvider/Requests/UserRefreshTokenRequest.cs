using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

// ReSharper disable CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.IdentityProvider
{
    public class UserRefreshTokenRequest
    {
        [JsonPropertyName("accessToken")]
        [Required]
        public string AccessToken { get; set; }

        [JsonPropertyName("refreshToken")]
        [Required]
        public string RefreshToken { get; set; }
    }
}
