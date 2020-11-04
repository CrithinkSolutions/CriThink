using System.Text.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.Admin
{
    public class DebunkingNewsGetAllResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("publisher")]
        public string Publisher { get; set; }
    }
}
