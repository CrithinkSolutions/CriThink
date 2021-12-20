using System.Text.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.NewsSource
{
    public class NewsSourceRelatedDebunkingNewsResponse : BaseNewsSourceSearch
    {
        [JsonPropertyName("publisher")]
        public string Publisher { get; set; }

        [JsonPropertyName("newsImageLink")]
        public string NewsImageLink { get; set; }

        [JsonPropertyName("caption")]
        public string Caption { get; set; }
    }
}
