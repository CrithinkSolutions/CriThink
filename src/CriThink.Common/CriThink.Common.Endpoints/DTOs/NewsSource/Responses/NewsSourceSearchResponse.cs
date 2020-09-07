using System.Text.Json.Serialization;

// Resharper disable CheckNamespace

namespace CriThink.Common.Endpoints.DTOs.NewsSource
{
    public class NewsSourceSearchResponse
    {
        [JsonPropertyName("classification")]
        public NewsSourceClassification Classification { get; set; }
    }
}
