using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.IdentityProvider
{
    public class UsernameAvailabilityRequest
    {
        [Required]
        [MinLength(2)]
        [JsonPropertyName("username")]
        public string Username { get; set; }
    }
}
