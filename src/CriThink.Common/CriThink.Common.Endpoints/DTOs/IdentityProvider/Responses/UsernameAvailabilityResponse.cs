using System.Text.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.IdentityProvider
{
    public class UsernameAvailabilityResponse
    {
        [JsonPropertyName("isAvailable")]
        public bool IsAvailable { get; set; }
    }
}
