using System.Text.Json.Serialization;

namespace CriThink.Common.Endpoints.DTOs.NewsSource
{
    public class NewsSourcePostAnswersResponse
    {
        [JsonPropertyName("userRate")]
        public decimal UserRate { get; set; }

        [JsonPropertyName("communityName")]
        public decimal CommunityRate { get; set; }

        [JsonPropertyName("newsSource")]
        public NewsSourceSearchWithDebunkingNewsResponse NewsSource { get; set; }
    }
}
