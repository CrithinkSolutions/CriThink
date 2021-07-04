using System.Text.Json.Serialization;

namespace CriThink.Common.Endpoints.DTOs.Statistics
{
    public class SearchesCountingResponse
    {
        [JsonPropertyName("totalSearches")]
        public long TotalSearches { get; set; }
    }
}
