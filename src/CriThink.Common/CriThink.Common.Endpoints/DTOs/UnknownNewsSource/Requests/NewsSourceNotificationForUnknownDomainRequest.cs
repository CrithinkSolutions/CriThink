using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.UnknownNewsSource
{
    public class NewsSourceNotificationForUnknownDomainRequest
    {
        [Required]
        [JsonPropertyName("newsSource")]
        public string NewsSource { get; set; }
    }
}
