using System.Text.Json.Serialization;

#pragma warning disable CA1056 // URI-like properties should not be strings

namespace CriThink.Common.Endpoints.DTOs.NewsAnalyzer.Responses
{
    public class DemoNewsResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}
