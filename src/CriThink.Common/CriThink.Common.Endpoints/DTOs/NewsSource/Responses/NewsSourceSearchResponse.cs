// Resharper disable CheckNamespace

using System.Text.Json.Serialization;

namespace CriThink.Common.Endpoints.DTOs.NewsSource
{
    public class NewsSourceSearchResponse
    {
        [JsonPropertyName("classification")]
        public NewsSourceClassification Classification { get; set; }
    }
}
