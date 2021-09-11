using System.Collections.Generic;
using System.Text.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.DebunkingNews
{
    public class DebunkingNewsGetDetailsResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("publisher")]
        public string Publisher { get; set; }

        [JsonPropertyName("publisherLanguage")]
        public string PublisherLanguage { get; set; }

        [JsonPropertyName("publisherCountry")]
        public string PublisherCountry { get; set; }

        [JsonPropertyName("publishingDate")]
        public string PublishingDate { get; set; }

        [JsonPropertyName("caption")]
        public string Caption { get; set; }

        [JsonPropertyName("link")]
        public string Link { get; set; }

        [JsonPropertyName("keywords")]
        public IReadOnlyList<string> Keywords { get; set; }

        [JsonPropertyName("imageLink")]
        public string ImageLink { get; set; }
    }
}
