using System.Text.Json.Serialization;

// ReSharper disable once CheckNamespace

namespace CriThink.Common.Endpoints.DTOs.NewsAnalyzer
{
    public class ScrapeNewsResponse
    {
        [JsonPropertyName("author")]
        public string Author { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("language")]
        public string Language { get; set; }

        [JsonPropertyName("readingMinutes")]
        public string ReadingMinutes { get; set; }

        [JsonPropertyName("publishingDate")]
        public string PublishingDate { get; set; }

        [JsonPropertyName("body")]
        public string Body { get; set; }

        [JsonPropertyName("website")]
        public string Website { get; set; }

        [JsonPropertyName("isSuccessfullyParsed")]
        public bool IsSuccessfullyParsed { get; set; }
    }
}
