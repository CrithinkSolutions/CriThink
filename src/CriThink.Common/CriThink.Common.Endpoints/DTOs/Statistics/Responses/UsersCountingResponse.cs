using System.Text.Json.Serialization;

namespace CriThink.Common.Endpoints.DTOs.Statistics
{
    public class UsersCountingResponse
    {
        [JsonPropertyName("usersCounting")]
        public long Counting { get; set; }
    }
}
