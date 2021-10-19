using System.Collections.Generic;
using System.Text.Json.Serialization;
using CriThink.Common.Endpoints.Converters;

namespace CriThink.Common.Endpoints.DTOs.NewsSource
{
    public class NewsSourcePostAnswersResponse
    {
        [JsonPropertyName("userRate")]
        public decimal? UserRate { get; set; }

        [JsonPropertyName("communityRate")]
        public decimal? CommunityRate { get; set; }

        [JsonPropertyName("classification")]
        [JsonConverter(typeof(NewsSourceClassificationConverter))]
        public NewsSourceAuthenticityDto Classification { get; set; }

        [JsonPropertyName("localizedClassification")]
        public string LocalizedClassification { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("relatedDebunkingNews")]
        public IReadOnlyCollection<NewsSourceRelatedDebunkingNewsResponse> RelatedDebunkingNews { get; set; }
    }
}
