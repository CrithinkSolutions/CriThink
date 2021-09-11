using System.Text.Json.Serialization;
using CriThink.Common.Endpoints.Converters;

// Resharper disable CheckNamespace

namespace CriThink.Common.Endpoints.DTOs.NewsSource
{
    public class NewsSourceSearchResponse
    {
        [JsonPropertyName("classification")]
        [JsonConverter(typeof(NewsSourceClassificationConverter))]
        public NewsSourceAuthenticityDto Classification { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
