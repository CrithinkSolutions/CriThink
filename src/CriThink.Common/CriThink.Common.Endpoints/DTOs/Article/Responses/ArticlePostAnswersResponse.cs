using System.Text.Json.Serialization;
using CriThink.Common.Endpoints.DTOs.NewsSource;

namespace CriThink.Common.Endpoints.DTOs.Article
{
    public class ArticlePostAnswersResponse
    {
        [JsonPropertyName("userRate")]
        public decimal UserRate { get; set; }

        [JsonPropertyName("communityName")]
        public decimal CommunityRate { get; set; }

        [JsonPropertyName("newsSource")]
        public NewsSourceSearchWithDebunkingNewsResponse NewsSource { get; set; }
    }
}
