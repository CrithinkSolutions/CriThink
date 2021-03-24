using System.Text.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.UnknownNewsSource
{
    public class NewsSourceNotificationForUnknownDomainRequest
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("uri")]
        public string Uri { get; set; }
    }
}
