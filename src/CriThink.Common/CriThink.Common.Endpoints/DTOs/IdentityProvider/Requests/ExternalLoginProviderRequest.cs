using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
// ReSharper disable CheckNamespace

namespace CriThink.Common.Endpoints.DTOs.IdentityProvider
{
    public class ExternalLoginProviderRequest
    {
        [Required]
        [JsonPropertyName("socialProvider")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ExternalLoginProvider SocialProvider { get; set; }

        [Required]
        [JsonPropertyName("userToken")]
        public string UserToken { get; set; }
    }
}
