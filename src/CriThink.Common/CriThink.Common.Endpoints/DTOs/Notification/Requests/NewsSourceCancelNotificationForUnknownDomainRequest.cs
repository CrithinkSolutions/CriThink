using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CriThink.Common.Endpoints.DTOs.Notification
{
    public class NewsSourceCancelNotificationForUnknownDomainRequest
    {
        [Required]
        [JsonPropertyName("newsSource")]
        public string NewsSource { get; set; }
    }
}
