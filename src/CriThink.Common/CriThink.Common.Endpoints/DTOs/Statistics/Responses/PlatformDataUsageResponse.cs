using System.Text.Json.Serialization;

namespace CriThink.Common.Endpoints.DTOs.Statistics
{
    public class PlatformDataUsageResponse
    {
        [JsonPropertyName("platformUsers")]
        public long PlatformUsers { get; set; }

        [JsonPropertyName("platformSearches")]
        public long PlatformSearches { get; set; }
    }
}
