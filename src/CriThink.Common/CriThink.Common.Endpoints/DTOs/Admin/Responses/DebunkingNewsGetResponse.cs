using System.Text.Json.Serialization;

namespace CriThink.Common.Endpoints.DTOs.Admin
{
    public class DebunkingNewsGetResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("publisher")]
        public string Publisher { get; set; }
    }
}