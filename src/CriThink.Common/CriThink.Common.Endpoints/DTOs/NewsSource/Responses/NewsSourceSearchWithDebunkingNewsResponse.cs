using System.Collections.Generic;
using System.Text.Json.Serialization;
using CriThink.Common.Endpoints.Converters;

namespace CriThink.Common.Endpoints.DTOs.NewsSource
{
    public class NewsSourceSearchWithDebunkingNewsResponse
    {
        [JsonPropertyName("classification")]
        [JsonConverter(typeof(NewsSourceClassificationConverter))]
        public NewsSourceClassification Classification { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("relatedDebunkingNews")]
        public IReadOnlyCollection<NewsSourceRelatedDebunkingNewsResponse> RelatedDebunkingNews { get; set; }
    }
}