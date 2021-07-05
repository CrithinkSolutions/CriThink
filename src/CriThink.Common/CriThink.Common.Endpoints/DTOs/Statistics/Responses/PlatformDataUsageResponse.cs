using System.Text.Json.Serialization;

namespace CriThink.Common.Endpoints.DTOs.Statistics
{
    public class PlatformDataUsageResponse
    {
        [JsonPropertyName("usersCounting")]
        public long Counting { get; set; }

        [JsonPropertyName("totalSearches")]
        public long TotalSearches { get; set; }
    }
}
