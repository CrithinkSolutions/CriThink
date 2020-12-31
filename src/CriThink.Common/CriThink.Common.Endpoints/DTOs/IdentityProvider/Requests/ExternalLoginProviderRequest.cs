using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
// ReSharper disable CheckNamespace

namespace CriThink.Common.Endpoints.DTOs.IdentityProvider
{
    public class ExternalLoginProviderRequest
    {
        [Required]
        [JsonPropertyName("socialProvider")]
        public ExternalLoginProvider SocialProvider { get; set; }

        [Required]
        [JsonPropertyName("userToken")]
        public string UserToken { get; set; }
    }
}
