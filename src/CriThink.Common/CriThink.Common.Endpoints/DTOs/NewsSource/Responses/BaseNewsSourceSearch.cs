using System.Text.Json.Serialization;

namespace CriThink.Common.Endpoints.DTOs.NewsSource
{
    public abstract class BaseNewsSourceSearch
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("newsLink")]
        public string NewsLink { get; set; }

        [JsonPropertyName("publishingDate")]
        public string PublishingDate { get; set; }
    }
}
