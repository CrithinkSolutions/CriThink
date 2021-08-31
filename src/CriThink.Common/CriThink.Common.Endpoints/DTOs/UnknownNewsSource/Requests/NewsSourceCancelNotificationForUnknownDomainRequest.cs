using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CriThink.Common.Endpoints.DTOs.UnknownNewsSource
{
    public class NewsSourceCancelNotificationForUnknownDomainRequest
    {
        [Required]
        [JsonPropertyName("newsSource")]
        public string NewsSource { get; set; }
    }
}
