using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.UnknownNewsSource
{
    public class NewsSourceNotificationForUnknownDomainRequest
    {
        [JsonPropertyName("email")]
        [Required]
        public string Email { get; set; }

        [Required]
        [JsonPropertyName("uri")]
        public string Uri { get; set; }
    }
}
