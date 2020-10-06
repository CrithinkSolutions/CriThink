using System.Text.Json.Serialization;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;

// ReSharper disable CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.Admin
{
    public class AdminSignUpResponse
    {
        [JsonPropertyName("jwtToken")]
        public JwtTokenResponse JwtToken { get; set; }

        [JsonPropertyName("adminId")]
        public string AdminId { get; set; }

        [JsonPropertyName("adminEmail")]
        public string AdminEmail { get; set; }
    }
}
