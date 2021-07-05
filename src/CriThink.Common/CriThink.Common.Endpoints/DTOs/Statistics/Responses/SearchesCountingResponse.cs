using System.Text.Json.Serialization;

namespace CriThink.Common.Endpoints.DTOs.Statistics
{
    public class SearchesCountingResponse
    {
        [JsonPropertyName("userSearches")]
        public long UserSearches { get; set; }
    }
}
