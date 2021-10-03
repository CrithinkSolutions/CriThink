using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CriThink.Common.Endpoints.DTOs.NewsSource;

namespace CriThink.Common.Endpoints.DTOs.NewsSource
{
    public class NewsSourceAddBatchRequest
    {
        [Required]
        [JsonPropertyName("newsSources")]
        public Dictionary<string, NewsSourceAuthenticityDto> NewsSources { get; set; }
    }
}
