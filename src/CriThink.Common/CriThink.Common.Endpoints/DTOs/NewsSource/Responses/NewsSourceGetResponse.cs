using System.Text.Json.Serialization;
using CriThink.Common.Endpoints.Converters;

// ReSharper disable once CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.NewsSource
{
    public class NewsSourceGetResponse
    {
        [JsonPropertyName("uri")]
        public string Uri { get; set; }

        [JsonPropertyName("classification")]
        [JsonConverter(typeof(NewsSourceClassificationConverter))]
        public NewsSourceClassification NewsSourceClassification { get; set; }
    }
}